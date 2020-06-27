using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.DTOs.ReportDTOs
{
    public class ReportDTO
    {
        public string MovieTitle { get; set; }
        public int Showtimes { get; set; }
        public int Tickets { get; set; }
        public int Revenue { get; set; }
    }
}
