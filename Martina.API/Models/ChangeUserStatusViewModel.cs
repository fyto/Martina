using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Models
{
    public class ChangeUserStatusViewModel
    {
        public Guid UserId { get; set; }

        public string UserStatus { get; set; }

        public int UserStatusId { get; set; }

        public string Comment { get; set; }

        public string OldStatus { get; set; }

        public int OldStatusId { get; set; }

        public DateTime DateChange { get; set; }

    }
}
