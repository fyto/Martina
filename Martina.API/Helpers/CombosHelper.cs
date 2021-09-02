using Martina.API.Data;
using Martina.Common.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;

        }

        public IEnumerable<SelectListItem> GetComboDeseases()
        {
            List<SelectListItem> list = _context.Deseases.Select(x => new SelectListItem
            {
                Text = x.Description,
                Value = x.Id.ToString()

            }).OrderBy(x => x.Text).ToList();


            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una enfermedad...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboUserTypes()
        {
            List<SelectListItem> list = _context.Roles.Select(x => new SelectListItem
            {

                Text = x.Name.ToString(),
                Value = x.Name.ToString()
            }).ToList();

            foreach (var item in list)
            {
                if (item.Text == "Admin")
                {
                    item.Text = "Administrador";
                }

                if (item.Text == "AdultoMayor")
                {
                    item.Text = "Adulto mayor";
                }
            }

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un tipo de usuario...]",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboDiseaseTypes()
        {
            List<SelectListItem> list = _context.DeseaseTypes.Select(x => new SelectListItem
            {

                Text = x.Description.ToString(),
                Value = x.Id.ToString()

            }).ToList();


            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un tipo de enfermedad...]",
                Value = "0"
            });

            return list;
        }
    }
}
