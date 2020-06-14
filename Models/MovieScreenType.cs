using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Models
{
    public class MovieScreenType
    {
        public int MovieId { get; set; }
        public int ScreenTypeId { get; set; }
        public Movie Movie { get; set; }
        public ScreenType ScreenType { get; set; }
    }
}
