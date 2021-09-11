using Martina.API.Data;
using Martina.API.Data.Entities;
using Martina.API.Helpers;
using Martina.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

        public UsersController(DataContext context, IUserHelper userHelper, ICombosHelper combosHelper, IConverterHelper converterHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
            //_mailHelper = mailHelper;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDisease(AddDiseaseViewModel model)
        {
            if(ModelState.IsValid)
            {
                //User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.User.Id);

                Disease disease = await _converterHelper.ToDiseaseAsync(model, true);

                _context.Deseases.Add(disease);
                await _context.SaveChangesAsync();
            }

            model.DiseaseTypes = _combosHelper.GetComboDiseaseTypes();
          

            return View(model);
        }

      
        public async Task<IActionResult> AddDisease()
        {
            AddDiseaseViewModel model = new AddDiseaseViewModel
            {
                DiseaseTypes = _combosHelper.GetComboDiseaseTypes()
            };

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





    }
}
