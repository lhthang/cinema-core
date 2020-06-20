using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Form
{
    public class UpdateMovieRequest
    {
        public string Title { get; set; }

        public List<int> ScreenTypeIds { get; set; }
        public List<int> GenreIds { get; set; }
        public string Poster { get; set; }
        public string EndAt { get; set; }
        public string[] Wallpapers { get; set; }
        public string Trailer { get; set; }
        public string Story { get; set; }
        public int RateId { get; set; }
    }
}
