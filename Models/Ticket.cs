using cinema_core.Models.Base;
using cinema_core.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Models
{
    public class Ticket : BaseEntity
    {
        public string Username { get; set; }
        public int ShowtimeId { get; set; }
        public virtual Showtime Showtime { get; set; }
        public string Seat { get; set; }
        public TicketType TicketType { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
