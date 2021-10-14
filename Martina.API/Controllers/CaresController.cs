using Martina.API.Data;
using Martina.API.Data.Entities;
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

        public CaresController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Cares.ToListAsync());
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
                    var careNew = new Care
                    {
                        Description = care
                    };

                    _context.Add(careNew);
                    await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index));
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
            //return View(care);
            return Json("Failed");
        }





        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Care care)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Add(care);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (DbUpdateException dbUpdateException)
        //        {
        //            if (dbUpdateException.InnerException.Message.Contains("duplicate"))
        //            {
        //                ModelState.AddModelError(string.Empty, "Ya existe este cuidado, verifique el nombre y vuelva a intentarlo.");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
        //            }

        //        }
        //        catch (Exception exception)
        //        {
        //            ModelState.AddModelError(string.Empty, exception.Message);
        //        }

        //    }
        //    return View(care);
        //}

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Care care)
        {
            if (id != care.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(care);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este cuidado.");
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
            return View(care);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var care = await _context.Cares
                .FirstOrDefaultAsync(m => m.Id == id);

            if (care == null)
            {
                return NotFound();
            }

            _context.Cares.Remove(care);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CareExists(int id)
        {
            return _context.Cares.Any(e => e.Id == id);
        }


    }
}
