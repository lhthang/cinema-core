using cinema_core.DTOs.TicketDTOs;
using cinema_core.ErrorHandle;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Models.Base;
using cinema_core.Repositories.Base;
using cinema_core.Repositories.Interfaces;
using cinema_core.Utils.Email;
using cinema_core.Utils.QR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class TicketRepository : BaseRepository, ITicketRepository
    {
        public TicketRepository(MyDbContext context) : base(context)
        {
        }

        public ICollection<TicketDTO> GetAllTicketsByShowtime(int showtimeId)
        {
            List<Ticket> tickets = dbContext.Tickets.Include(x => x.Showtime).Where(x => x.ShowtimeId == showtimeId).ToList();
            return tickets.Select(x => new TicketDTO(x)).ToList();
        }

        public TicketDTO GetTicketById(int id)
        {
            var ticket = GetTicketEntityById(id);
            return new TicketDTO(ticket);
        }

        public TicketDTO BuyTicket(TicketRequest ticketRequest)
        {
            ticketRequest.Seat = ticketRequest.Seat.ToUpper();

            CheckTicketValid(ticketRequest);

            var ticket = new Ticket()
            {
                Username = ticketRequest.Username,
                ShowtimeId = ticketRequest.ShowtimeId,
                TicketType = ticketRequest.TicketType,
                Seat = ticketRequest.Seat,
                ModifiedAt = DateTime.Now,
            };

            dbContext.Add(ticket);
            Save();
            Thread thread = new Thread(()=> EmailService.SendEmail(MyQRCode.GenerateQRCodeImage(ticket.Id)));
            thread.Start();
            return new TicketDTO(ticket);
        }


        public TicketDTO UpdateTicket(int id, TicketRequest ticketRequest)
        {
            ticketRequest.Seat = ticketRequest.Seat.ToUpper();

            CheckTicketValid(ticketRequest);

            var ticket = GetTicketEntityById(id);

            ticket.Username = ticketRequest.Username;
            ticket.ShowtimeId = ticketRequest.ShowtimeId;
            ticket.TicketType = ticketRequest.TicketType;
            ticket.Seat = ticketRequest.Seat;
            ticket.ModifiedAt = DateTime.Now;

            dbContext.Update(ticket);
            Save();
            return new TicketDTO(ticket);

        }

        public bool DeleteTicket(int id)
        {
            var ticketToDelete = GetTicketEntityById(id);
            dbContext.Remove(ticketToDelete);
            Save();
            return true;
        }

        private Ticket GetTicketEntityById(int id)
        {
            var ticket = dbContext.Tickets.Where(r => r.Id == id).FirstOrDefault();
            if (ticket == null)
                throw new CustomException(HttpStatusCode.NotFound, "Id not found.");

            return ticket;
        }

        private void CheckTicketValid(TicketRequest ticketRequest)
        {
            var showtime = dbContext.Showtime.Include(x => x.Room).FirstOrDefault(x => x.Id == ticketRequest.ShowtimeId);
            if (showtime == null)
                throw new CustomException(HttpStatusCode.NotFound,"Cannot find Showtime.");

            string seatStr = ticketRequest.Seat;
            // XYY with:    X: an alphabet letter;    Y: a digit
            // Valid: A01, A1, B00, B99. Invalid: AB01, A100
            var seatRegex = new Regex(@"^[A-Z]\d{1,2}$");
            if (!seatRegex.IsMatch(seatStr))
                throw new CustomException(HttpStatusCode.BadRequest, "Wrong seat format.");

            int alphabetPart = Convert.ToInt32(seatStr[0]); // ASCII
            int digitPart = int.Parse(seatStr.Substring(1));

            if (alphabetPart < 65 || alphabetPart > 64 + showtime.Room.TotalRows)
                throw new CustomException(HttpStatusCode.BadRequest, "Invalid row of Seat.");

            if (digitPart < 1 || digitPart > showtime.Room.TotalSeatsPerRow)
                throw new CustomException(HttpStatusCode.BadRequest, "Invalid column of Seat.");

            var ticketsOfShowtime = dbContext.Tickets.Where(x => x.ShowtimeId == ticketRequest.ShowtimeId).ToList();
            foreach (var ticketOfShowtime in ticketsOfShowtime)
			{
                int alphabet = Convert.ToInt32(ticketOfShowtime.Seat[0]);
                int digit = int.Parse(ticketOfShowtime.Seat.Substring(1));

                if (alphabetPart == alphabet && digitPart == digit)
                    throw new CustomException(HttpStatusCode.BadRequest, "Seat already occupied.");
            }

            //var ticketBySeat = dbContext.Tickets.FirstOrDefault(x => x.ShowtimeId == ticketRequest.ShowtimeId && x.Seat == ticketRequest.Seat);
            //if (ticketBySeat != null)
            //    throw new Exception("Seat already occupied");
        }
    }
}
