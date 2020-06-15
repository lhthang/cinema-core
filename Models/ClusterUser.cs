using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Models
{
    public class ClusterUser
    {
        public int ClusterId { get; set; }
        public int UserId { get; set; }
        public Cluster Cluster { get; set; }
        public User User { get; set; }
    }
}
