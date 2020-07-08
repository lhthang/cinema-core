using cinema_core.DTOs.TicketDTOs;
using cinema_core.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        public ICollection<TicketDTO> GetAllTicketsByShowtime(int showtimeId);
        public TicketDTO GetTicketById(int id);
        public List<TicketDTO> BuyTickets(string username, TicketRequest ticketRequest);
        //public TicketDTO UpdateTicket(int id, TicketRequest ticketRequest);
        public bool DeleteTicket(int id);

        public List<TicketDTO> GetAllTicketsByShowtimeId(int id);
    }
}
