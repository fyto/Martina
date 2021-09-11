using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Martina.API.Models
{
    public class AddDiseaseViewModel
    {
        //public int  Id { get; set; }

        [Display(Name = "Enfermedad")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        [Display(Name = "Tipo de enfermedad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int? DiseaseTypeId { get; set; }

        public IEnumerable<SelectListItem> DiseaseTypes { get; set; }
    }
}
