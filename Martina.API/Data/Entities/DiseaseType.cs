using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Data.Entities
{
    public class DiseaseType
    {
        public int Id { get; set; }

        [Display(Name = "Tipo enfermedad")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        public bool State { get; set; }
    }
}
