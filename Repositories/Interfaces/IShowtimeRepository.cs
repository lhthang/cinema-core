using cinema_core.DTOs.ShowtimeDTOs;
using cinema_core.Form;
using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories
{
    public interface IShowtimeRepository
    {
        ICollection<ShowtimeDTO> GetAllShowtimes(int skip, int limit);
        Showtime GetShowtimeById(int id);

        Showtime CreateShowtime(ShowtimeRequest showtimeRequest);

        Showtime UpdateShowtime(int id, ShowtimeRequest showtimeRequest);

        bool DeleteShowtime(Showtime showtime);

        void AutoUpdateShowtime();

        bool Save();
    }
}
