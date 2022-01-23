using Martina.API.Data;
using Martina.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Controllers
{
    public class UserStatusController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        public UserStatusController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpPost]
        public async Task<JsonResult> GetPossibleStatusByStatus(int userStatusId)
        {
            // Estado del usuario
            var statusUserModal = _context.UserStatus.Where(x => x.Id == userStatusId).FirstOrDefault();

            // Estado registrado
            var statusInitial = _context.UserStatus.Where(y => y.Id == 1).FirstOrDefault();

            var statusUsers = _context.UserStatus.ToList();

            statusUsers.Remove(statusUserModal);
            statusUsers.Remove(statusInitial);

            return Json(statusUsers);
           
        }
    }
}
