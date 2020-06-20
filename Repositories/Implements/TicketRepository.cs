using cinema_core.DTOs.TicketDTOs;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Models.Base;
using cinema_core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class TicketRepository : ITicketRepository
    {
        private MyDbContext dbContext;

        public TicketRepository(MyDbContext context)
        {
            dbContext = context;
        }

        private bool Save()
        {
            return dbContext.SaveChanges() > 0;
        }

        public ICollection<TicketDTO> GetAllTicketsByShowtime(int showtimeId)
        {
            List<Ticket> tickets = dbContext.Tickets.Include(x => x.Showtime).Where(x => x.ShowtimeId == showtimeId).ToList();
            return tickets.Select(x => new TicketDTO(x)).ToList();
        }

        public TicketDTO GetTicketById(int id)
        {
            var ticket = dbContext.Tickets.Include(x => x.Showtime).FirstOrDefault(x => x.Id == id);
            return new TicketDTO(ticket);
        }

        public TicketDTO BuyTicket(TicketRequest ticketRequest)
        {
            var ticketBySeat = dbContext.Tickets.FirstOrDefault(x => x.ShowtimeId == ticketRequest.ShowtimeId && x.Seat == ticketRequest.Seat);
            if (ticketBySeat != null)
            {
                // TODO: Throw message "Seat already occupied"
                return null;
            }

            var ticket = new Ticket()
            {
                Username = ticketRequest.Username,
                ShowtimeId = ticketRequest.ShowtimeId,
                TicketType = ticketRequest.TicketType,
                Seat = ticketRequest.Seat,
                CreatedAt = DateTime.Now,
            };

            dbContext.Add(ticket);
            var isSuccess = Save();
            if (!isSuccess) return null;
            return new TicketDTO(ticket);
        }

        //public TicketDTO UpdateTicket(int id, TicketRequest ticketRequest)
        //{
        //    var ticket = dbContext.Tickets.Where(r => r.Id == id).FirstOrDefault();
        //    if (ticket == null)
        //        return null;
        //    ticket.Username = ticketRequest.Username;

        //    dbContext.Update(ticket);
        //    var isSuccess = Save();
        //    if (!isSuccess) return null;
        //    return new TicketDTO(ticket);

        //}

        public bool DeleteTicket(int id)
        {
            var ticketToDelete = dbContext.Tickets.FirstOrDefault(x => x.Id == id);
            if (ticketToDelete == null)
                return false;

            dbContext.Remove(ticketToDelete);
            return Save();
        }

        //public TicketDTO GetTicketByName(string name)
        //{
        //    var ticket = dbContext.Tickets.Where(g => g.Name == name).FirstOrDefault();
        //    return new TicketDTO(ticket);
        //}
    }
}
