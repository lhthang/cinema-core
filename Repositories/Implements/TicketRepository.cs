using cinema_core.DTOs.TicketDTOs;
using cinema_core.ErrorHandle;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Models.Base;
using cinema_core.Repositories.Base;
using cinema_core.Repositories.Interfaces;
using cinema_core.Utils.Constants;
using cinema_core.Utils.Email;
using cinema_core.Utils.Enums;
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

        public List<TicketDTO> BuyTickets(string username, TicketRequest ticketRequest)
        {
            ticketRequest.Seats = FormatSeats(ticketRequest.Seats);

            CheckTicketValid(ticketRequest);

            List<Ticket> ticketsToAdd = new List<Ticket>();

            foreach (var seat in ticketRequest.Seats)
            {
                var promotionFound = dbContext.Promotions.FirstOrDefault(x => x.Code == seat.PromotionCode && x.IsActive == true);
                var ticket = new Ticket()
                {
                    Username = username,
                    ShowtimeId = ticketRequest.ShowtimeId,
                    TicketType = seat.TicketType,
                    Seat = seat.Seat,
                    PromotionId = promotionFound != null ? promotionFound.Id : (int?)null,
                    Price = GetTicketPrice(ticketRequest.ShowtimeId, seat.TicketType, promotionFound),
                    ModifiedAt = DateTime.Now,
                };
                ticketsToAdd.Add(ticket);
            }

            dbContext.AddRange(ticketsToAdd);
            Save();

            List<int> ticketIds = ticketsToAdd.Select((x) => x.Id).ToList();
            var user = dbContext.Users.Where(u => u.Username == username).FirstOrDefault();
            var showtime = dbContext.Showtime.Where(s => s.Id == ticketRequest.ShowtimeId).Include(m => m.Movie).FirstOrDefault();
            Thread thread = new Thread(()=> EmailService.SendEmail(MyQRCode.GenerateQRCodeImage(ticketIds),user,showtime.Movie));
            thread.Start();

            List<TicketDTO> resultList = ticketsToAdd.Select((x) => new TicketDTO(x)).ToList();
            return resultList;
        }


        //public TicketDTO UpdateTicket(int id, TicketRequest ticketRequest)
        //{
        //    ticketRequest.Seats = FormatSeats(ticketRequest.Seats);

        //    CheckTicketValid(ticketRequest);

        //    var ticket = GetTicketEntityById(id);

        //    ticket.ShowtimeId = ticketRequest.ShowtimeId;
        //    ticket.TicketType = ticketRequest.TicketType;
        //    ticket.Seat = ticketRequest.Seat;
        //    ticket.ModifiedAt = DateTime.Now;

        //    dbContext.Update(ticket);
        //    Save();
        //    return new TicketDTO(ticket);

        //}

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

        private List<TicketSeatRequest> FormatSeats(List<TicketSeatRequest> seats)
        {
            // Ex: Convert a1 to A01
            List<TicketSeatRequest> resultList = seats.Select((seat) =>
            {
                string alphabetPart = seat.Seat[0].ToString();
                int digitPart = int.Parse(seat.Seat.Substring(1));
                string digitPartStr;
                if (digitPart < 10)
                {
                    digitPartStr = digitPart.ToString("00");
                }
                else
                {
                    digitPartStr = digitPart.ToString();
                }

                string newSeatStr = alphabetPart + digitPartStr;
                newSeatStr = newSeatStr.ToUpper();

                return new TicketSeatRequest()
                {
                    Seat = newSeatStr,
                    TicketType = seat.TicketType,
                    PromotionCode = seat.PromotionCode
                };
            }).ToList();

            return resultList;
        }

        private void CheckTicketValid(TicketRequest ticketRequest)
        {
            var showtime = dbContext.Showtime.Include(x => x.Room).FirstOrDefault(x => x.Id == ticketRequest.ShowtimeId);
            if (showtime == null)
                throw new CustomException(HttpStatusCode.BadRequest,"Cannot find Showtime.");

            foreach (var seat in ticketRequest.Seats)
            {
                if (!string.IsNullOrEmpty(seat.PromotionCode))
                {
                    var promotion = dbContext.Promotions.FirstOrDefault(x => x.Code == seat.PromotionCode && x.IsActive == true);
                    if (promotion == null)
                        throw new CustomException(HttpStatusCode.BadRequest, "Promotion not found.");
                }
            }

            if (ticketRequest.Seats.Count <= 0)
            {
                throw new CustomException(HttpStatusCode.BadRequest, "Seats cannot be empty.");
            }

            foreach (var seat in ticketRequest.Seats)
            {
                string seatStr = seat.Seat;

                if (!Enum.IsDefined(typeof(TicketType), seat.TicketType))
                    throw new CustomException(HttpStatusCode.BadRequest, $"Invalid ticket type: {seatStr}.");

                // XYY with:    X: an alphabet letter;    Y: a digit
                // Valid: A01, A1, B00, B99. Invalid: AB01, A100
                var seatRegex = new Regex(@"^[A-Z]\d{1,2}$");
                if (!seatRegex.IsMatch(seatStr))
                    throw new CustomException(HttpStatusCode.BadRequest, $"Wrong seat format: {seatStr}.");

                int alphabetPart = Convert.ToInt32(seatStr[0]); // ASCII
                int digitPart = int.Parse(seatStr.Substring(1));

                if (alphabetPart < 65 || alphabetPart > 64 + showtime.Room.TotalRows)
                    throw new CustomException(HttpStatusCode.BadRequest, $"Invalid row of Seat: {seatStr}.");

                if (digitPart < 1 || digitPart > showtime.Room.TotalSeatsPerRow)
                    throw new CustomException(HttpStatusCode.BadRequest, $"Invalid column of Seat: {seatStr}.");

                var ticketsOfShowtime = dbContext.Tickets.Where(x => x.ShowtimeId == ticketRequest.ShowtimeId).ToList();
                foreach (var ticketOfShowtime in ticketsOfShowtime)
                {
                    int alphabet = Convert.ToInt32(ticketOfShowtime.Seat[0]);
                    int digit = int.Parse(ticketOfShowtime.Seat.Substring(1));

                    if (alphabetPart == alphabet && digitPart == digit)
                        throw new CustomException(HttpStatusCode.BadRequest, $"Seat already occupied: {seatStr}.");
                }
            }

   //         string seatStr = ticketRequest.Seat;
   //         // XYY with:    X: an alphabet letter;    Y: a digit
   //         // Valid: A01, A1, B00, B99. Invalid: AB01, A100
   //         var seatRegex = new Regex(@"^[A-Z]\d{1,2}$");
   //         if (!seatRegex.IsMatch(seatStr))
   //             throw new CustomException(HttpStatusCode.BadRequest, "Wrong seat format.");

   //         int alphabetPart = Convert.ToInt32(seatStr[0]); // ASCII
   //         int digitPart = int.Parse(seatStr.Substring(1));

   //         if (alphabetPart < 65 || alphabetPart > 64 + showtime.Room.TotalRows)
   //             throw new CustomException(HttpStatusCode.BadRequest, "Invalid row of Seat.");

   //         if (digitPart < 1 || digitPart > showtime.Room.TotalSeatsPerRow)
   //             throw new CustomException(HttpStatusCode.BadRequest, "Invalid column of Seat.");

   //         var ticketsOfShowtime = dbContext.Tickets.Where(x => x.ShowtimeId == ticketRequest.ShowtimeId).ToList();
   //         foreach (var ticketOfShowtime in ticketsOfShowtime)
			//{
   //             int alphabet = Convert.ToInt32(ticketOfShowtime.Seat[0]);
   //             int digit = int.Parse(ticketOfShowtime.Seat.Substring(1));

   //             if (alphabetPart == alphabet && digitPart == digit)
   //                 throw new CustomException(HttpStatusCode.BadRequest, "Seat already occupied.");
   //         }
        }

        private decimal GetTicketPrice(int showtimeId, TicketType ticketType, Promotion promotion)
        {
            var showtime = dbContext.Showtime.FirstOrDefault(x => x.Id == showtimeId);
            decimal ticketPrice = showtime.BasePrice;

            if (promotion != null)
            {
                ticketPrice = ticketPrice - promotion.DiscountAmount > 0 ? ticketPrice - promotion.DiscountAmount : 0;
            }

            // Temp Hardcode. TODO: Save ticketType's price somewhere else
            switch (ticketType)
            {
                case TicketType.Child:
                    ticketPrice *= 1;
                    break;
                case TicketType.Adult:
                    ticketPrice *= (decimal)0.8;
                    break;
            }
            return ticketPrice;
        }

        public List<TicketDTO> GetAllTicketsByShowtimeId(int id)
        {
            List<TicketDTO> ticketDTOs = new List<TicketDTO>();
            var tickets = dbContext.Tickets.Where(t => t.ShowtimeId == id).ToList();
            foreach(var ticket in tickets)
            {
                ticketDTOs.Add(new TicketDTO(ticket));
            }
            return ticketDTOs;
        }
    }
}
