using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Data.Entities
{
    public class UserDisease
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int DiseaseId { get; set; }

        public string DiseaseName { get; set; }

        public User User { get; set; }

        public Disease Disease { get; set; }
    }
}
