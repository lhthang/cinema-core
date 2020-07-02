using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Form
{
    public class ShowtimeRequest
    {
        public string Status { get; set; }
        [Required]
        public string StartAt { get; set; }
        [Required]
        public decimal BasePrice { get; set; }
        [Required]
        public int MovieId { get; set; }
        [Required]
        public int RoomId { get; set; }
        [Required]
        public int ScreenTypeId { get; set; }
    }
}
