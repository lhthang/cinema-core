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
        public string Username { get; set; } // TEMP. TODO: Get by token
        [Required]
        public int ShowtimeId { get; set; }
        [Required]
        public string Seat { get; set; }
        [Required]
        public TicketType TicketType { get; set; }
    }
}
