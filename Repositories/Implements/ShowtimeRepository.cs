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

        public ICollection<ShowtimeDTO> GetAllShowtimes(int skip, int limit)
        {
            List<ShowtimeDTO> results = new List<ShowtimeDTO>();
            List<Showtime> showtimes = dbContext.Showtime
                                            .Include(m => m.Movie)
                                            .Include(r => r.Room)
                                            .Include(st => st.ScreenType)
                                            .OrderBy(c => c.Id).Skip(skip).Take(limit)
                                            .ToList();
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
            return null;
        }

        public Showtime UpdateShowtime(int id, ShowtimeRequest showtimeRequest)
        {
            return null;
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
    }
}
