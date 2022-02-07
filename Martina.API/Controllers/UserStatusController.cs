using Martina.API.Data;
using Martina.API.Data.Entities;
using Martina.API.Helpers;
using Martina.API.Models;
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
        public JsonResult GetPossibleStatusByStatus(int userStatusId)
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

        [HttpPost]
        public async Task<JsonResult> ChangeUserStatus(ChangeUserStatusViewModel changeUserStatus)
        {
            // User 
            var user = await _userHelper.GetUserAsync(changeUserStatus.UserId);

            if (user == null)
            {
                return Json("Not found");
            }

            try
            {
                // Actualiza estado en User
                user.UserStatusId = changeUserStatus.UserStatusId;
                user.UserStatus = changeUserStatus.UserStatus;
                await _userHelper.UpdateUserAsync(user);

                // Crea el registro en HistoryUserStatus
                HistoryUserStatus historyUserStatus = new HistoryUserStatus
                {
                    UserId = changeUserStatus.UserId,
                    OldState = changeUserStatus.OldStatus,
                    OldStateId = changeUserStatus.OldStatusId,
                    NewState = changeUserStatus.UserStatus,
                    NewStateId = changeUserStatus.UserStatusId,
                    Comment = changeUserStatus.Comment,
                    DateChange = DateTime.Now
                };

                _context.HistoryUsersStatus.Add(historyUserStatus);
                await _context.SaveChangesAsync();

                return Json("Success");
            }
            catch (Exception error)
            {
                return Json(error.Message);
            }

        }

    }
}
