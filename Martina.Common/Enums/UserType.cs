using System.ComponentModel.DataAnnotations;

namespace Martina.Common.Enums
{
    public enum UserType
    {
        [Display(Name = "Administrador")]
        Admin,
        [Display(Name = "Apoderado")]
        Apoderado,
        [Display(Name = "Adulto mayor")]
        AdultoMayor,
        [Display(Name = "Cuidador")]
        Cuidador
    }
}
