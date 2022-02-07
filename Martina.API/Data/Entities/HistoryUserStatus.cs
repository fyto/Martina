using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Data.Entities
{
    public class HistoryUserStatus
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public string OldState { get; set; }

        public int OldStateId { get; set; }

        public string NewState { get; set; }

        public int NewStateId { get; set; }

        public string Comment { get; set; }

        public DateTime DateChange { get; set; }
    }
}
