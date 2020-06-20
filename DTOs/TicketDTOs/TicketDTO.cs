using cinema_core.DTOs.ShowtimeDTOs;
using cinema_core.Models;
using cinema_core.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.DTOs.TicketDTOs
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public ShowtimeDTO Showtime { get; set; }
        public string Seat { get; set; }
        public TicketType TicketType { get; set; }
        public DateTime ModifiedAt { get; set; }

        public TicketDTO(Ticket ticket)
        {
            Username = ticket.Username;
            Showtime = new ShowtimeDTO(ticket.Showtime);
            Seat = ticket.Seat;
            TicketType = ticket.TicketType;
            ModifiedAt = ticket.ModifiedAt;
        }
    }
}
