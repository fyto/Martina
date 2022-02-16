using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Models.Request
{
    public class UserRequest
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Email { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; }


        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PhoneNumber { get; set; }

        public byte[] Image { get; set; }

        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        //public string UserType { get; set; }

        //[Display(Name = "Id tipo usuario")]
        //public string UserTypeId { get; set; }

        //[Display(Name = "Id estado de usuario")]
        //public int UserStatusId { get; set; }

        //[Display(Name = "Estado de usuario")]
        //public string UserStatus { get; set; }

        //[Display(Name = "Usuario")]
        //public string FullName => $"{FirstName} {LastName}";
    }
}
