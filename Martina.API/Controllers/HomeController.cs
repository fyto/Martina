using Martina.API.Data;
using Martina.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger,
                              DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Ejemplo de Join con Linq

            //var result = ( from user in _context.UsersDiseases
            //               join desease in _context.Deseases
            //               on user.DiseaseId equals desease.Id                         
            //               select new { User = user, Diseases = desease });

            //foreach (var item in result)
            //{
            //    var User = item.User;
            //    var Diseases = item.Diseases;

            //    string message = User.FirstName + " = " + Diseases.DescriptionDeseaseType;

            //}

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       
        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }
    }
}
