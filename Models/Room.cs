using cinema_core.Models.Base;
using System.Collections.Generic;

namespace cinema_core.Models
{
    public class Room : BaseEntity
    {

        public string Name { get; set; }

        public int TotalSeatsPerRow { get; set; }

        public int TotalRows { get; set; }

        public virtual ICollection<RoomScreenType> RoomScreenTypes { get; set; }
        //public virtual ClusterRoom ClusterRoom { get; set; }
        public int ClusterId { get; set; }
        public Cluster Cluster { get; set; }
        public virtual ICollection<Showtime> Showtimes { get; set; }
    }
}
