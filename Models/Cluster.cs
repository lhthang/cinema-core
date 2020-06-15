using cinema_core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Models
{
    public class Cluster : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        //public int UserId { get; set; }
        //public virtual User User { get; set; }
        //public virtual ICollection<ClusterRoom> ClusterRooms { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ClusterUser ClusterUser { get; set; }
    }
}
