using cinema_core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Models
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
