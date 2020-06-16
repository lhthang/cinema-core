﻿using cinema_core.DTOs.RateDTOs;
using cinema_core.Models;
using cinema_core.Models.Base;
using cinema_core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class RateRepository : IRateRepository
    {
        private MyDbContext dbContext;
        public RateRepository(MyDbContext context)
        {
            this.dbContext = context;
        }

        public bool CreateRate(Rate rate)
        {
            dbContext.Add(rate);
            return Save();
        }

        public bool DeleteRate(Rate rate)
        {
            dbContext.Remove(rate);
            return Save();
        }

        public ICollection<RateDTO> GetAllRates(int skip, int limit)
        {
            var rates = dbContext.Rates.OrderBy(r => r.Id).Skip(skip).Take(limit).ToList();
            List<RateDTO> rateDTOs = new List<RateDTO>();
            foreach(var rate in rates)
            {
                rateDTOs.Add(new RateDTO(rate));
            }
            return rateDTOs;
        }

        public Rate GetRateById(int id)
        {
            var rate = dbContext.Rates.Where(r => r.Id == id).FirstOrDefault();
            return rate;
        }

        public Rate GetRateByName(string name)
        {
            var rate = dbContext.Rates.Where(r => r.Name == name).FirstOrDefault();
            return rate;
        }

        public bool Save()
        {
            return dbContext.SaveChanges() > 0;
        }
    }
}
