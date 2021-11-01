using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Martina.API.Data.Entities
{
    public class Disease
    {
        public int Id { get; set; }     

        [Display(Name = "Tipo enfermedad")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }


        [Display(Name = "Tipo de enfermedad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DiseaseType DiseaseType { get; set; }

        [Display(Name = "Nombre Tipo enfermedad")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string DescriptionDeseaseType { get; set; }

        public int DiseaseTypeId { get; set; }




    }
}
