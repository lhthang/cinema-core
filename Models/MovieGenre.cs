using cinema_core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Models
{
    public class MovieGenre : BaseEntity
    {
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
