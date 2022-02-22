using Common.Models;
using Martina.API.Data.Entities;
using Martina.API.Helpers;
using Martina.API.Models;
using Martina.API.Models.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Martina.API.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IBlobHelper _blobHelper;
        private readonly IMailHelper _mailHelper;


        public AccountController(IUserHelper userHelper,
                                IConfiguration configuration,
                                IBlobHelper blobHelper,
                                IMailHelper mailHelper)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _blobHelper = blobHelper;
            _mailHelper = mailHelper;
        }


        [HttpPost]
        [Route("CreateToken")]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Username);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.ValidatePasswordAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        Claim[] claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        JwtSecurityToken token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(90),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            user
                        };

                        return Created(string.Empty, results);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<User>> PostUser(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await _userHelper.GetUserAsync(request.Email);
            if (user != null)
            {
                return BadRequest("Ya existe un usuario registrado con  ese email.");
            }

            Guid imageId = Guid.Empty;
            if (request.Image != null && request.Image.Length > 0)
            {
                imageId = await _blobHelper.UploadBlobAsync(request.Image, "users");
            }

            user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Address = request.Address,
                ImageId = imageId,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email,
                UserType = request.UserType,
                UserStatus = request.UserStatus
            };

            await _userHelper.AddUserAsync(user, request.Password);
            await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());

            string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            string tokenLink = Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            _mailHelper.SendMail(user.Email, "Vehicles - Confirmación de cuenta", $"<h1>Vehicles - Confirmación de cuenta</h1>" +
                $"Para habilitar el usuario, " +
                $"por favor hacer clic en el siguiente enlace: </br></br><a href = \"{tokenLink}\">Confirmar Email</a>");

            return Ok(user);
        }

        [HttpPost]
        [Route("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Email);
                if (user == null)
                {
                    return BadRequest("El correo ingresado no corresponde a ningún usuario.");
                }

                string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                string link = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);
                _mailHelper.SendMail(model.Email, "Vehicles - Reseteo de contraseña", $"<h1>Vehicles - Reseteo de contraseña</h1>" +
                    $"Para establecer una nueva contraseña haga clic en el siguiente enlace:</br></br>" +
                    $"<a href = \"{link}\">Cambio de Contraseña</a>");
                return Ok("Las instrucciones para el cambio de contraseña han sido enviadas a su email.");
            }

            return BadRequest(model);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task<IActionResult> PutUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

          
            //Guid imageId = user.ImageId;

            //if (request.ImageArray != null)
            //{
            //    imageId = await _blobHelper.UploadBlobAsync(request.ImageArray, "users");
            //}

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Address = request.Address;
            user.PhoneNumber = request.PhoneNumber;
           
            //user.ImageId = imageId;
            //user.Latitude = request.Latitude;
            //user.Logitude = request.Logitude;

            IdentityResult respose = await _userHelper.UpdateUserAsync(user);
            if (!respose.Succeeded)
            {
                return BadRequest(respose.Errors.FirstOrDefault().Description);
            }

            User updatedUser = await _userHelper.GetUserAsync(email);
            return Ok(updatedUser);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            IdentityResult result = await _userHelper.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Error005"
                });
            }

            return Ok(new Response { IsSuccess = true });
        }


    }
}
