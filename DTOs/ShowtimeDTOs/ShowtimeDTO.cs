using cinema_core.DTOs.MovieDTOs;
using cinema_core.DTOs.RoomDTOs;
using cinema_core.DTOs.ScreenTypeDTOs;
using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.DTOs.ShowtimeDTOs
{
    public class ShowtimeDTO
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public decimal BasePrice { get; set; }
        public MovieDTO Movie { get; set; }
        public RoomDTO Room { get; set; }
        public ScreenTypeDTO ScreenType { get; set; }

        public ShowtimeDTO(Showtime showtime)
        {
            if (showtime == null)
                return;

            this.Id = showtime.Id;
            this.Status = showtime.Status;
            this.StartAt = showtime.StartAt;
            this.BasePrice = showtime.BasePrice;
            this.EndAt = showtime.EndAt;
            this.Movie = new MovieDTO(showtime.Movie);
            this.Room = new RoomDTO(showtime.Room);
            this.ScreenType = new ScreenTypeDTO(showtime.ScreenType);
        }
    }
}
