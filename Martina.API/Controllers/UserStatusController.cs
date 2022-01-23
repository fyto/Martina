using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Controllers
{
    public class UserStatusController : Controller
    {
        [HttpPost]
        public async Task<JsonResult> GetUserStatusByStatus(int userStatusId)
        {

            return Json("hola");
            //return Json(await _context.DeseaseTypes.ToListAsync());
        }
    }
}
