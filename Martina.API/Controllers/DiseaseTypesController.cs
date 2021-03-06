using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Martina.API.Data;
using Martina.API.Data.Entities;
using System.Collections.Generic;
using Martina.API.Models;

namespace Martina.API.Controllers
{
    public class DiseaseTypesController : Controller
    {
        private readonly DataContext _context;

        public DiseaseTypesController(DataContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<JsonResult> GetDiseaseTypes()
        {
            return Json(await _context.DeseaseTypes.ToListAsync());
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetDiseases(int? idDiseaseType)
        {
            if (idDiseaseType != null)
            {
                return Json(await _context.Deseases.Where(u => u.DiseaseTypeId == idDiseaseType).ToListAsync());
            }
            else
            {
                return Json(await _context.Deseases.ToListAsync());
            }

        }


        [HttpPost]
        public async Task<JsonResult> GetDiseasesByUser(string id)
        {
            List<DiseasesByUserViewModel> DiseasesByUser = new List<DiseasesByUserViewModel>();

            var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            var diseases = await _context.Deseases.ToListAsync();

            var diseasesUser = await _context.UsersDiseases.Where(y => y.UserId == id).ToListAsync();
                   
            foreach (var disease in diseases)
            {
                bool flag = false;

                foreach (var diseaseUser in diseasesUser)
                {                      
                    if (disease.Id == diseaseUser.DiseaseId)
                    {
                        flag = true;
                    }  
                }

                if (!flag)
                {
                    DiseasesByUser.Add(new DiseasesByUserViewModel()
                    {
                        UserId = id,
                        DiseaseId = disease.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        DiseaseName = disease.Description,
                        DiseasedStatus = false
                    });
                }
                else
                {
                    DiseasesByUser.Add(new DiseasesByUserViewModel()
                    {
                        UserId = id,
                        DiseaseId = disease.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        DiseaseName = disease.Description,
                        DiseasedStatus = true
                    });
                }
            }    

            return Json(DiseasesByUser);  
        }


        [HttpPost]
        public async Task<JsonResult> CreateDiseaseType(string nameDiseaseType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var diseaseTypeNew = new DiseaseType
                    {
                        Description = nameDiseaseType                     
                    };

                    _context.Add(diseaseTypeNew);
                    await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index));
                    return Json("Success");
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este tipo de enfermedad, verifique el nombre y vuelva a intentarlo.");
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
                    return Json("Error");
                }

            }
            return Json("Failed");
        }

        [HttpPost]
        public async Task<JsonResult> CreateDisease(int id, string nameDisease)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var diseaseType = _context.DeseaseTypes.Where(i => i.Id == id).FirstOrDefault();

                    var disease = new Disease
                    {                      
                        Description = nameDisease,
                        DiseaseType = diseaseType,
                        DescriptionDeseaseType = diseaseType.Description
                    };

                    _context.Add(disease);
                    await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index));
                    return Json("Success");
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este tipo de enfermedad, verifique el nombre y vuelva a intentarlo.");
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
                    return Json("Error");
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

            var diseaseType = await _context.DeseaseTypes.FindAsync(id);
            if (diseaseType == null)
            {
                return NotFound();
            }
            return View(diseaseType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  DiseaseType diseaseType)
        {
            if (id != diseaseType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diseaseType);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este tipo de enfermedad.");
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
            return View(diseaseType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diseaseType = await _context.DeseaseTypes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (diseaseType == null)
            {
                return NotFound();
            }

            _context.DeseaseTypes.Remove(diseaseType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

      
    }
}
