using Martina.API.Data;
using Martina.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Controllers
{
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        //private readonly IUserHelper _userHelper;
        //private readonly ICombosHelper _combosHelper;
        //private readonly IConverterHelper _converterHelper;
        //private readonly IBlobHelper _blobHelper;
        //private readonly IMailHelper _mailHelper;

        public UsersController(DataContext context)
        {
            _context = context;
            //_userHelper = userHelper;
            //_combosHelper = combosHelper;
            //_converterHelper = converterHelper;
            //_blobHelper = blobHelper;
            //_mailHelper = mailHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users
                                .Include(x => x.Diseases)
                                .Where(x => x.UserType == UserType.Admin)
                                .ToListAsync());
        }
    }
}
