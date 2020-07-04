using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Utils.MovieProxy
{
    public class MovieResponse
    {
        public string Title { get; set; }
        public string Imdb { get; set; }
        public List<Actor> Actors { get; set; }
        public string[] Directors { get; set; }
        public string Country { get; set; }
        public string[] Languages { get; set; }
        public int Runtime { get; set; }
        public string Poster { get; set; }
        public List<Genre> Genres { get; set; }
        public string RateName { get; set; }
        public DateTime ReleasedAt { get; set; }
    }
}
