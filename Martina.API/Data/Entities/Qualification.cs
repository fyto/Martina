using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Martina.API.Data.Entities
{
    public class Qualification
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime Date { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime DateLocal => Date.ToLocalTime();

        //[JsonIgnore]
        //public Product Product { get; set; }

        //public User User { get; set; }

        public User UserQualified { get; set; }

        public string UserQualifyingId { get; set; }       

        //public string QualifiedUserId { get; set; }

        //[JsonIgnore]
        //public User UserQualifications { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public float Score { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
    }
}
