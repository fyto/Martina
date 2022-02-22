using Common.Models;
using Martina.API.Data;
using Martina.API.Data.Entities;
using Martina.API.Helpers;
using Martina.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IMailHelper _mailHelper;

        public UsersController(DataContext context, IUserHelper userHelper, 
                               ICombosHelper combosHelper, 
                               IConverterHelper converterHelper,
                               IBlobHelper blobHelper,
                               IMailHelper mailHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;

            _mailHelper = mailHelper;
        }


        [HttpPost]
        public async Task<JsonResult> GetUsers()
        {
            return Json(await _context.Users.ToListAsync());
        }

        [HttpPost]
        public async Task<JsonResult> GetUser(string id)
        {
            var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            
            return Json(user);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users
                                .ToListAsync());
        }

        [HttpPost]
        public async Task<JsonResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    //imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _converterHelper.ToUserAsync(model, imageId, true);

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendMail(model.Email, "App - Confirmación de cuenta", $"<h1>App - Confirmación de cuenta</h1>" +
                    $"Para habilitar el usuario, " +
                    $"por favor hacer clic en el siguiente enlace: </br></br><a href = \"{tokenLink}\">Confirmar Email</a>");

                //return RedirectToAction(nameof(Index));
                return Json("Success");
            }

            //model.UserTypes = _combosHelper.GetComboUserTypes();
            //return View(model);
            return Json("Error");
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(Guid.Parse(id));
            if (user == null)
            {
                return NotFound();
            }

            UserViewModel model = await _converterHelper.ToUserViewModel(user);

            return View(model);
        }      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;
                if (model.ImageFile != null)
                {
                    //imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _converterHelper.ToUserAsync(model, imageId, false);
                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }

            //model.UserTypes = _combosHelper.GetComboUserTypes();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddDiseaseToUser([FromBody]DiseasesByUserViewModel diseasesByUser)
        {
            var userDisease = _converterHelper.ToUserDisease(diseasesByUser);

            // Se elimina la asociación entre el usuario y la enfermedad
            if (diseasesByUser.DiseasedStatus)
            {
                try
                {
                    _context.UsersDiseases.Remove(userDisease.Result);
                  
                    await _context.SaveChangesAsync();

                    //return Json("Remove Success");

                }
                catch (DbUpdateException dbUpdateException)
                {
                    return Json(dbUpdateException.InnerException.Message);
                }              
            }
            else // Se agrega la asociación entre usuario y la enfermedad
            {
                try
                {
                    _context.UsersDiseases.Add(userDisease.Result);
                    
                    await _context.SaveChangesAsync();

                    //return Json("Add Success");
                }
                catch (DbUpdateException dbUpdateException)
                {
                    return Json(dbUpdateException.InnerException.Message);
                }

            }

            List<DiseasesByUserViewModel> DiseasesByUser = new List<DiseasesByUserViewModel>();

            var user = await _context.Users.Where(x => x.Id == diseasesByUser.UserId).FirstOrDefaultAsync();

            var diseases = await _context.Deseases.ToListAsync();

            var diseasesUser = await _context.UsersDiseases.Where(y => y.UserId == diseasesByUser.UserId).ToListAsync();

            foreach (var disease in diseases)
            {
                bool flag = false;

                foreach (var diseaseUser in diseasesUser)
                {
                    if (disease.Id == diseaseUser.DiseaseId)
                    {
                        flag = true;
                    }
                }

                if (!flag)
                {
                    DiseasesByUser.Add(new DiseasesByUserViewModel()
                    {
                        UserId = diseasesByUser.UserId,
                        DiseaseId = disease.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        DiseaseName = disease.Description,
                        DiseasedStatus = false
                    });
                }
                else
                {
                    DiseasesByUser.Add(new DiseasesByUserViewModel()
                    {
                        UserId = diseasesByUser.UserId,
                        DiseaseId = disease.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        DiseaseName = disease.Description,
                        DiseasedStatus = true
                    });
                }
            }

            return Json(DiseasesByUser);

        }

        public async Task<IActionResult> AddDisease()
        {
            Collection<Disease> collection = new Collection<Disease>();

            var DeseasesList = await _context.Deseases.Include(x => x.DiseaseType).ToListAsync();

            foreach (var item in DeseasesList)
            {
                collection.Add(item);
            }

            AddDiseaseViewModel model = new AddDiseaseViewModel
            {
                DiseaseTypes = _combosHelper.GetComboDiseaseTypes(),
                Diseases = collection
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDisease(AddDiseaseViewModel model)
        {
            if(ModelState.IsValid)
            {
                //User user = await _context.Users
                //                   .Include(x => x.Diseases)
                //                   .FirstOrDefaultAsync(x => x.Id == model.UserId);

                //if (user == null)
                //{
                //    return NotFound();
                //}

                Disease disease = await _converterHelper.ToDiseaseAsync(model, true);

                try
                {
                    _context.Deseases.Add(disease);
                    await _context.SaveChangesAsync();

                    //_notyf.Custom("Enfermedad creada correctamente.", 3, "green", "fa fa-user-md");

                    return RedirectToAction(nameof(AddDisease));

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        //ModelState.AddModelError(string.Empty, "Ya existe una enfermedad con ese nombre.");
                        //_notyf.Error("Ya existe una enfermedad con ese nombre.", 3);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

            }

            model.DiseaseTypes = _combosHelper.GetComboDiseaseTypes();
          

            return View(model);
        }   

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(Guid.Parse(id));
            if (user == null)
            {
                return NotFound();
            }

            await _blobHelper.DeleteBlobAsync(user.ImageId, "users");
            await _userHelper.DeleteUserAsync(user);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> AssociateDisease(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(Guid.Parse(id));
            if (user == null)
            {
                return NotFound();
            }

            return View();
        }




    }
}
