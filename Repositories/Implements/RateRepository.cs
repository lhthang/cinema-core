using cinema_core.DTOs.RateDTOs;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Models.Base;
using cinema_core.Repositories.Base;
using cinema_core.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class RateRepository : BaseRepository, IRateRepository
    {
        public RateRepository(MyDbContext context) : base(context) {
        }

        public RateDTO GetRateById(int id)
        {
            var rate = GetRateEntityById(id);
            return new RateDTO(rate);
        }

        public ICollection<RateDTO> GetRates(int skip, int limit)
        {
            List<RateDTO> results = new List<RateDTO>();
            var rates = dbContext.Rates.OrderBy(sc => sc.Id).Skip(skip).Take(limit).ToList();
            foreach (Rate rate in rates)
            {
                results.Add(new RateDTO(rate));
            }
            return results;
        }

        public RateDTO CreateRate(RateRequest rateRequest)
        {
            CheckRateValid(rateRequest);
            var rate = new Rate()
            {
                Name = rateRequest.Name,
                MinAge = rateRequest.MinAge,
            };

            dbContext.Add(rate);
            Save();
            return new RateDTO(rate);
        }

        public RateDTO UpdateRate(int id, RateRequest rateRequest)
        {
            CheckRateValid(rateRequest);
            var rate = GetRateEntityById(id);

            rate.Name = rateRequest.Name;
            rate.MinAge = rateRequest.MinAge;

            dbContext.Update(rate);
            Save();
            return new RateDTO(rate);
        }

        public bool DeleteRate(int id)
        {
            var rateToDelete = GetRateEntityById(id);

            dbContext.Remove(rateToDelete);
            Save();
            return true;
        }

        private Rate GetRateEntityById(int id)
        {
            var rate = dbContext.Rates.Where(r => r.Id == id).FirstOrDefault();
            if (rate == null)
                throw new Exception("Id not found.");

            return rate;
        }

        private void CheckRateValid(RateRequest rateRequest)
		{
            if (rateRequest.MinAge < 0 || rateRequest.MinAge > 99)
                throw new Exception("Rate must be between 0 and 99");
		}
    }
}
