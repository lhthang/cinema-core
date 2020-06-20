using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.DTOs.RateDTOs
{
    public class RateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int MinAge { get; set; }
        public RateDTO(Rate rate)
        {
            if (rate == null)
                return;

            Id = rate.Id;
            Name = rate.Name;
            MinAge = rate.MinAge;
        }
    }
}
