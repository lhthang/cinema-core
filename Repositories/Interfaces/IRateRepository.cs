using cinema_core.DTOs.RateDTOs;
using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Interfaces
{
    public interface IRateRepository
    {
        public ICollection<RateDTO> GetAllRates();
        public Rate GetRateById(int id);

        public bool DeleteRate(Rate rate);
        public bool Save();
    }
}
