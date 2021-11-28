using Martina.API.Data;
using Martina.API.Data.Entities;
using Martina.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Controllers
{
    
    public class CaresController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;


        public CaresController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;           
        }

        [HttpPost]
        public async Task<JsonResult> GetCares()
        {
            return Json(await _context.Cares.ToListAsync());
        }

        public IActionResult Create()
        {  
            return View();
        }
       

        [HttpPost]
        public async Task<JsonResult> Create(string care)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userEmail = User.Identity.Name;

                    var user = await _userHelper.GetUserAsync(userEmail);

                    var careNew = new Care
                    {
                        Description = care,
                        CreationDate = DateTime.Now,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    };

                    _context.Add(careNew);
                    await _context.SaveChangesAsync();
                  
                    return Json("Success");
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        //ModelState.AddModelError(string.Empty, "Ya existe este cuidado, verifique el nombre y vuelva a intentarlo.");
                        return Json("Duplicate");
                    }
                    else
                    {
                        //ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                        return Json(dbUpdateException.InnerException.Message);
                    }

                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                    return Json(exception.Message);
                }

            }
        
            return Json("Failed");
        }


        [HttpPost]
        public async Task<JsonResult> Edit([FromBody]Care care)
        {
            //if (id != care.Id)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {                

                    _context.Update(care);
                    await _context.SaveChangesAsync();
                 
                    return Json("Success");
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este cuidado.");
                        return Json("Duplicate");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                        return Json(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                    return Json(exception.Message);
                }
            }
            return Json("Failed");

           

        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var care = await _context.Cares.FindAsync(id);
            if (care == null)
            {
                return NotFound();
            }
            return View(care);
        }     

        [HttpPost]
        public async Task<JsonResult> Delete(int? id)
        {
            if (id == null)
            {
                return Json("Error");
            }

            var care = await _context.Cares
                .FirstOrDefaultAsync(m => m.Id == id);

            if (care == null)
            {
                return Json("NoExist");
            }

            _context.Cares.Remove(care);
            await _context.SaveChangesAsync();
           
            return Json("Success");
        }    

    }
}
