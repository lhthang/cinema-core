using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.DTOs.ReportDTOs
{
    public class ReportDTO
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int Showtimes { get; set; }
        public int Tickets { get; set; }
        public decimal Revenue { get; set; }
    }
}
