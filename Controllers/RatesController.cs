using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cinema_core.DTOs.RateDTOs;
using cinema_core.Models;
using cinema_core.Repositories.Interfaces;
using cinema_core.Utils.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cinema_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatesController : Controller
    {
        private IRateRepository rateRepository;
        public RatesController(IRateRepository repository)
        {
            rateRepository = repository;
        }

        [HttpGet()]
        [AllowAnonymous]
        public IActionResult Get(int skip = 0,int limit=10000)
        {
            if (limit <= 0)
            {
                Error error = new Error() { Message = "Limit must be greater than 0" };
                return StatusCode(400, error);
            }
            var rates = rateRepository.GetAllRates(skip,limit);
            return Ok(rates);
        }

        [HttpGet("{id}", Name = "GetRate")]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            var rate = rateRepository.GetRateById(id);
            if (rate == null)
                return NotFound();
            return Ok(new RateDTO(rate));
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public IActionResult Delete(int id)
        {
            var rate = rateRepository.GetRateById(id);
            if (rate == null)
                return NotFound();
            if (!rateRepository.DeleteRate(rate))
            {
                Error error = new Error() { Message = "Something went wrong when delete rate" };
                return StatusCode(400, error);
            }
            return Ok(new RateDTO(rate));
        }

        [HttpPost()]
        [AllowAnonymous]
        public IActionResult Post([FromBody] Rate rate)
        {
            var isExist = rateRepository.GetRateByName(rate.Name);
            if (isExist != null)
            {
                Error error = new Error() { Message = $"Rate {rate.Name} is exist" };
                return StatusCode(400, error);
            }

            if (!ModelState.IsValid) return StatusCode(400, ModelState);

            if (!rateRepository.CreateRate(rate))
            {
                Error error = new Error() { Message = "Something went wrong when save rate" };
                return StatusCode(400, error);
            }
            return RedirectToRoute("GetRate",new { id = rate.Id });
        }
    }
}