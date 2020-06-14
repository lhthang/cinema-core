using cinema_core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Models
{
    public class Actor : BaseEntity
    {
        public string Name { get; set; }
        public string Avatar { get; set; }

        public virtual ICollection<MovieActor> MovieActors { get; set; }
    }
}
