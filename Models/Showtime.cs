using cinema_core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Models
{
    public class Showtime : BaseEntity
    {
        public string Status { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public decimal BasePrice { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int ScreenTypeId { get; set; }
        public ScreenType ScreenType { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

    }
}
