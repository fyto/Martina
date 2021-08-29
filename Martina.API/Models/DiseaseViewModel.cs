using Martina.API.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Models
{
    public class DiseaseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de vehículo")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un tipo de verhículo.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int DiseaseTypeId { get; set; }

        public IEnumerable<SelectListItem> DiseaseTypes { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Tipo enfermedad")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }


    }
}
