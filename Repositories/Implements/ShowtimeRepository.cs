using cinema_core.DTOs.ShowtimeDTOs;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Models.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class ShowtimeRepository : IShowtimeRepository
    {
        private MyDbContext dbContext;

        public ShowtimeRepository(MyDbContext context)
        {
            dbContext = context;
        }

        public ICollection<ShowtimeDTO> GetAllShowtimes(int skip, int limit, int clusterId)
        {
            List<ShowtimeDTO> results = new List<ShowtimeDTO>();
            List<Showtime> showtimes = new List<Showtime>();
            showtimes = dbContext.Showtime
                                            .Include(m => m.Movie)
                                            .Include(r => r.Room).ThenInclude(c=>c.Cluster)
                                            .Include(st => st.ScreenType)
                                            .OrderBy(c => c.Id).Skip(skip).Take(limit)
                                            .ToList();

            var cluster = dbContext.Clusters.Where(c => c.Id == clusterId).FirstOrDefault();

            if (cluster != null)
            {
                showtimes = showtimes.Where(s => s.Room.ClusterId == clusterId).ToList();
            }
            foreach (Showtime showtime in showtimes)
            {
                results.Add(new ShowtimeDTO(showtime));
            }
            return results;
        }
        public Showtime GetShowtimeById(int id)
        {
            Showtime showtime = dbContext.Showtime
                                    .Where(s => s.Id == id)
                                    .Include(m => m.Movie)
                                    .Include(r => r.Room)
                                    .Include(st => st.ScreenType)
                                    .FirstOrDefault();
            return showtime;
        }

        public Showtime CreateShowtime(ShowtimeRequest showtimeRequest)
        {
            Showtime showtime = new Showtime();
            showtime.Status = showtimeRequest.Status;
            showtime.StartAt = DateTime.Parse(showtimeRequest.StartAt);
            showtime.Movie = dbContext.Movies.Where(m => m.Id == showtimeRequest.MovieId).FirstOrDefault();
            if (showtime.Movie != null)
            {
                showtime.EndAt = showtime.StartAt.AddMinutes(showtime.Movie.Runtime);
            }
            showtime.BasePrice = showtimeRequest.BasePrice;
            showtime.Room = dbContext.Rooms.Where(r => r.Id == showtimeRequest.RoomId).FirstOrDefault();
            showtime.ScreenType = dbContext.ScreenTypes.Where(st => st.Id == showtimeRequest.ScreenTypeId).FirstOrDefault();
            dbContext.Add(showtime);
            bool isSuccess = Save();
            if (!isSuccess)
            {
                return null;
            }
            return showtime;
        }

        public Showtime UpdateShowtime(int id, ShowtimeRequest showtimeRequest)
        {
            Showtime showtime = dbContext.Showtime.Where(s => s.Id == id).FirstOrDefault();
            showtime.Status = showtimeRequest.Status;
            showtime.StartAt = DateTime.Parse(showtimeRequest.StartAt);
            showtime.Movie = dbContext.Movies.Where(m => m.Id == showtimeRequest.MovieId).FirstOrDefault();
            if (showtime.Movie != null)
            {
                showtime.EndAt = showtime.StartAt.AddMinutes(showtime.Movie.Runtime);
            }
            showtime.BasePrice = showtimeRequest.BasePrice;
            showtime.Room = dbContext.Rooms.Where(r => r.Id == showtimeRequest.RoomId).FirstOrDefault();
            showtime.ScreenType = dbContext.ScreenTypes.Where(st => st.Id == showtimeRequest.ScreenTypeId).FirstOrDefault();
            dbContext.Update(showtime);
            bool isSuccess = Save();
            if (!isSuccess)
            {
                return null;
            }
            return showtime;
        }

        public bool DeleteShowtime(Showtime showtime)
        {
            dbContext.Remove(showtime);
            return Save();
        }

        public bool Save()
        {
            return dbContext.SaveChanges() > 0;
        }

        public void AutoUpdateShowtime()
        {
            var startOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var endOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            var showtimes = dbContext.Showtime.Where(s => s.StartAt.CompareTo(endOfDay) <= 0&&s.Status=="OPEN")
                .OrderByDescending(c => c.StartAt).ToList();

            foreach( var showtime in showtimes)
            {
                System.Diagnostics.Debug.WriteLine(showtime.StartAt);
                if (showtime.StartAt.CompareTo(DateTime.Now) <= 0)
                {
                    showtime.Status = "CLOSE";
                    dbContext.Update(showtime);
                }
            }
            Save();
        }
    }
}
