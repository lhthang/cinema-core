using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Form
{
    public class ClusterRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        //public List<int> RoomIds { get; set; }
        public int? ManagerId { get; set; }
    }
}
