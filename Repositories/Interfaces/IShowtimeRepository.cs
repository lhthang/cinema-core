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

        ICollection<ShowtimeDTO> GetShowtimesByClusterId(int clusterId);

        ICollection<ShowtimeDTO> GetShowtimesByClusterIdAndMovieId(int clusterId, int movieId);

        ICollection<ShowtimeDTO> GetShowtimesByRoomId(int roomId);
        Showtime GetShowtimeById(int id);

        Showtime CreateShowtime(ShowtimeRequest showtimeRequest);

        Showtime UpdateShowtime(int id, ShowtimeRequest showtimeRequest);

        bool DeleteShowtime(Showtime showtime);

        void AutoUpdateShowtime();

        bool Save();
    }
}
