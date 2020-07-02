using cinema_core.Utils.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Form
{
    public class TicketRequest
    {
        [Required]
        public int ShowtimeId { get; set; }
        //[Required]
        //public List<string> Seats { get; set; }
        //[Required]
        //public TicketType TicketType { get; set; }

        public List<TicketSeatRequest> Seats { get; set; }
    }

    public class TicketSeatRequest
    {
        public string Seat { get; set; }
        public TicketType TicketType { get; set; }
    }
}
