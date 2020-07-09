using cinema_core.DTOs.ReportDTOs;
using cinema_core.ErrorHandle;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Models.Base;
using cinema_core.Repositories.Base;
using cinema_core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class ReportRepository : BaseRepository, IReportRepository
    {

        public ReportRepository(MyDbContext context) : base(context)
        {
        }
        public ICollection<ReportDTO> GetReport(ReportRequest request)
        {
            List<ReportDTO> reports = new List<ReportDTO>();
            DateTime date = DateTime.Parse(request.Date);
            if (date == null) throw new CustomException(HttpStatusCode.BadRequest, "invalid date time");

            DateTime start = new DateTime(date.Year, date.Month, 1);
            DateTime end = start.AddMonths(1).AddDays(-1);

            var movies = dbContext.Movies.Where(m => m.ReleasedAt.CompareTo(end) <= 0 || m.EndAt.CompareTo(start) >= 0).ToList();

            foreach (var movie in movies)
            {
                var showtimes = dbContext.Showtime.Where(
                    s => s.MovieId == movie.Id
                    && s.StartAt.CompareTo(start) >= 0 && s.EndAt.CompareTo(end.AddDays(1)) < 0).ToList();
                var result = (from showtime in showtimes
                              join ticket in dbContext.Tickets on showtime.Id equals ticket.ShowtimeId
                              where showtime.MovieId == movie.Id
                              group ticket by ticket.ShowtimeId into r
                              select new
                              {
                                  showtimeId = r,
                                  movieId = movie.Id,
                                  tickets = r.Count(),
                                  //TODO get total price when ticket has price
                                  totalPrice = r.Sum(t => t.Price),
                              }).ToList();

                var report = new ReportDTO()
                {
                    MovieId = movie.Id,
                    Movie = movie.Title,
                    Showtimes = showtimes.Count(),
                };
                decimal totalPrice = 0;
                int totalTickets = 0;
                foreach (var r in result)
                {
                    totalPrice += r.totalPrice;
                    totalTickets += r.tickets;
                }
                report.Tickets = totalTickets;
                report.TotalPrice = totalPrice;
                reports.Add(report);
            }
            return reports;
        }
    }
}
