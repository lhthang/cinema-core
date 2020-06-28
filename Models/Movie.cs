using cinema_core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Models
{
    public class Movie : BaseEntity
    {
        public string[] Directors { get; set; }

        public string[] Languages { get; set; }
        public string Story { get;set; }
        public string Imdb { get; set; }

        public string Country { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
        public string[] Wallpapers { get; set; }
        public int Runtime { get; set; }
        public DateTime ReleasedAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Trailer { get; set; }

        public virtual ICollection<MovieScreenType> MovieScreenTypes { get; set; }

        public virtual ICollection<MovieActor> MovieActors { get; set; }
        public virtual ICollection<MovieGenre> MovieGenres { get; set; }

        public virtual Rate Rate { get; set; }

        public virtual ICollection<Showtime> Showtimes { get; set; }

    }
}
