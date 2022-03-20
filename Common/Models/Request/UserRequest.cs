using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Models.Request
{
    public class UserRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string UserType { get; set; }

        [Display(Name = "Estado de usuario")]
        public string UserStatus { get; set; }

        //[Display(Name = "Id tipo usuario")]
        //public string UserTypeId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        public byte[] ImageArray { get; set; }

        public string PasswordConfirm { get; set; }

        //public double Latitude { get; set; }

        //public double Logitude { get; set; }

    }
}
