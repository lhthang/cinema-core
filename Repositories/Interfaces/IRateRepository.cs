using cinema_core.DTOs.RateDTOs;
using cinema_core.Form;
using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Interfaces
{
    public interface IRateRepository
    {
        ICollection<RateDTO> GetRates(int skip, int limit);
        RateDTO GetRateById(int Id);
        RateDTO CreateRate(RateRequest rateRequest);
        RateDTO UpdateRate(int id, RateRequest rateRequest);
        bool DeleteRate(int id);
    }
}
