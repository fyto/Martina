using AspNetCoreHero.ToastNotification.Abstractions;
using Martina.API.Data;
using Martina.API.Data.Entities;
using Martina.API.Helpers;
using Martina.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Martina.API.Controllers
{
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;
        //private readonly IMailHelper _mailHelper;

        private readonly INotyfService _notyf;

        public UsersController(DataContext context, IUserHelper userHelper, 
                               ICombosHelper combosHelper, IConverterHelper converterHelper,
                               IBlobHelper blobHelper, INotyfService notyf)
        {
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
            //_mailHelper = mailHelper;

            _notyf = notyf;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users
                                .Include(x => x.Diseases)
                                .ToListAsync());
        }

        public IActionResult Create()
        {
            UserViewModel model = new UserViewModel
            {  
                UserTypes = _combosHelper.GetComboUserTypes()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _converterHelper.ToUserAsync(model, imageId, true);
             
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());

                //string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                //string tokenLink = Url.Action("ConfirmEmail", "Account", new
                //{
                //    userid = user.Id,
                //    token = myToken
                //}, protocol: HttpContext.Request.Scheme);

                //Response response = _mailHelper.SendMail(model.Email, "Vehicles - Confirmación de cuenta", $"<h1>Vehicles - Confirmación de cuenta</h1>" +
                //    $"Para habilitar el usuario, " +
                //    $"por favor hacer clic en el siguiente enlace: </br></br><a href = \"{tokenLink}\">Confirmar Email</a>");

                return RedirectToAction(nameof(Index));
            }

            //model.UserTypes = _combosHelper.GetComboUserTypes();
            return View(model);
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
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _converterHelper.ToUserAsync(model, imageId, false);
                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }

            model.UserTypes = _combosHelper.GetComboUserTypes();
            return View(model);
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

                    _notyf.Custom("Enfermedad creada correctamente.", 3, "green", "fa fa-user-md");

                    return RedirectToAction(nameof(AddDisease));

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        //ModelState.AddModelError(string.Empty, "Ya existe una enfermedad con ese nombre.");
                        _notyf.Error("Ya existe una enfermedad con ese nombre.", 3);
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
                .Include(x => x.Diseases)
                .ThenInclude(x => x.DiseaseType)
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
