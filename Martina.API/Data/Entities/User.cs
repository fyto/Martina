using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Martina.API.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        public string Address { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        [Display(Name = "Nombre foto")]
        public string ImageName { get; set; }

        // TODO: Fix the images path
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
          ? $"https://martina.blob.core.windows.net/users/noimage.png"
           : $"https://martina.blob.core.windows.net/users/{ImageId}";
        
        [Display(Name = "Tipo de usuario")]
        public string UserType { get; set; }

        [Display(Name = "Id tipo usuario")]
        public string UserTypeId { get; set; }

        [Display(Name = "Id estado de usuario")]
        public int UserStatusId { get; set; }

        [Display(Name = "Estado de usuario")]
        public string UserStatus { get; set; }

        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";

        [JsonIgnore]
        public ICollection<UserDisease> UsersDiseases { get; set; }

        [JsonIgnore]
        public ICollection<History> HistoryUsersStatus { get; set; }

        [JsonIgnore]
        public ICollection<Qualification> Qualifications { get; set; }

        [DisplayName("Calificación usuario")]
        public int UsersQualifications => Qualifications == null ? 0 : Qualifications.Count;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public float Qualification => Qualifications == null || Qualifications.Count == 0 ? 0 : Qualifications.Average(q => q.Score);


    }
}
