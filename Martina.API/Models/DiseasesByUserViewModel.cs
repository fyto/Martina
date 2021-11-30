using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Models
{
    public class DiseasesByUserViewModel
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public int DiseaseId { get; set; }

        public string DiseaseName { get; set; }

        public bool DiseasedStatus { get; set; }
    }
}
