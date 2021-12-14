using Martina.API.Data;
using Martina.API.Data.Entities;
using Martina.API.Helpers;
using Martina.API.Models;
using Martina.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;  
        private readonly IMailHelper _mailHelper;

        public AccountController(IUserHelper userHelper, 
                                 DataContext context, 
                                 IBlobHelper blobHelper,
                                 IMailHelper mailHelper)
        {
            _userHelper = userHelper;
            _context = context;
            _blobHelper = blobHelper;
            _mailHelper = mailHelper;
        }


        public IActionResult Login()
        {


            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<JsonResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        var returnUrl = Request.Query["ReturnUrl"].First();

                        return Json("Hola cabros");
                    }

                    //return RedirectToAction("Index", "Home");
                    return Json("Index/Home");
                }

                return Json("Email o contraseña incorrectos");
                //ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            }
            return Json("Model invalid");
            //return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _userHelper.AddUserAsync(model, imageId);

                if (user == null)
                {
                    //ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado por otro usuario.");
                    //return View(model);
                    return Json("Email repeat");
                }

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendMail(model.Username, "App - Confirmación de cuenta", $"<h1>App - Confirmación de cuenta</h1>" +
                    $"Para habilitar el usuario, " +
                    $"por favor hacer clic en el siguiente enlace: </br></br><a href = \"{tokenLink}\">Confirmar Email</a>");

                if (response.IsSuccess)
                {
                    //ViewBag.Message = "Las instrucciones para habilitar su cuenta han sido enviadas al correo.";
                    //return View(model);
                    return Json("Email send");
                }

                //ModelState.AddModelError(string.Empty, response.Message);

               
            }

            //model.DocumentTypes = _combosHelper.GetComboDocumentTypes();
            return Json("Model invalid");
        }

        public IActionResult ChangeUser()
        {
            return View();
        }
    
        [HttpPost]
        public async Task<JsonResult> EditUser()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return Json("Not found");
            }

            UserViewModel model = new()
            {
                Address = user.Address,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ImageId = user.ImageId,
                Id = user.Id,
                Email = user.Email,
                UserType = user.UserType
            };

            return new JsonResult(model);
        }

        [HttpPost]
        public async Task<JsonResult> ChangeUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _userHelper.GetUserAsync(User.Identity.Name);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.ImageId = imageId;
                
                await _userHelper.UpdateUserAsync(user);

                //return RedirectToAction("Index", "Home");
                return Json("Success");
            }

            //model.DocumentTypes = _combosHelper.GetComboDocumentTypes();
            //return View(model);
            return Json("Model invalid");
        }

        [HttpPost]
        public async Task<JsonResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(User.Identity.Name);
                if (user != null)
                {
                    IdentityResult result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        //return RedirectToAction(nameof(ChangeUser));
                        return Json("Success");
                    }
                    else
                    {
                        return Json("Incorrect password");                       
                        //ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    return Json("User not found");
                    //ModelState.AddModelError(string.Empty, "Usuario no encontrado.");
                }
            }

            return Json("Model invalid");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(new Guid(userId));
            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Email);
                if (user == null)
                {
                    //ModelState.AddModelError(string.Empty, "El correo ingresado no corresponde a ningún usuario.");
                    //return View(model);
                    return Json("Email invalid");
                }

                string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                string link = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);
                _mailHelper.SendMail(model.Email, "App - Reseteo de contraseña", $"<h1>App - Reseteo de contraseña</h1>" +
                    $"Para establecer una nueva contraseña haga clic en el siguiente enlace:</br></br>" +
                    $"<a href = \"{link}\">Cambio de Contraseña</a>");
                //ViewBag.Message = "Las instrucciones para el cambio de contraseña han sido enviadas a su email.";
                //return View();
                return Json("Success");

            }

            return Json("Model invalid");
            //return View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ResetPassword(ResetPasswordViewModel model)
        {
            User user = await _userHelper.GetUserAsync(model.UserName);
            if (user != null)
            {
                IdentityResult result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    //ViewBag.Message = "Contraseña cambiada.";
                    //return View();
                    return Json("Success");
                }

                //ViewBag.Message = "Error cambiando la contraseña.";
                //return View(model);
                return Json("Error");
            }

            //ViewBag.Message = "Usuario no encontrado.";
            //return View(model);
            return Json("User invalid");
        }

    }
}
