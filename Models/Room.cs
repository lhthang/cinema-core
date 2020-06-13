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
    }
}
