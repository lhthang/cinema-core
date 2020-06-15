using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cinema_core.DTOs.RateDTOs;
using cinema_core.Repositories.Interfaces;
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
                ModelState.AddModelError("", "Something went wrong when delete rate");
                return StatusCode(400, ModelState);
            }
            return Ok(new RateDTO(rate));
        }
    }
}