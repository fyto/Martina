using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboDeseases();

        IEnumerable<SelectListItem> GetComboUserTypes();

        IEnumerable<SelectListItem> GetComboDiseaseTypes();

        //IEnumerable<SelectListItem> GetComboVehicleTypes();

        //IEnumerable<SelectListItem> GetComboBrands();
    }
}
