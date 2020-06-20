using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cinema_core.DTOs.RateDTOs;
using cinema_core.Form;
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

        // GET: api/rates
        [HttpGet]
        public IActionResult Get()
        {
            var rates = rateRepository.GetRates();
            return Ok(rates);
        }

        // GET: api/rates/5
        [HttpGet("{id}", Name = "GetRate")]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            try
            {
                var rate = rateRepository.GetRateById(id);
                return Ok(rate);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        // POST: api/rates
        [HttpPost]
        public IActionResult Post([FromBody] RateRequest rateRequest)
        {
            if (rateRequest == null)
                return StatusCode(400, ModelState);
            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            try
            {
                var rate = rateRepository.CreateRate(rateRequest);
                return Ok(rate);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        // PUT: api/rates/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RateRequest rateRequest)
        {
            if (rateRequest == null)
                return StatusCode(400, ModelState);
            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            try
            {
                var rate = rateRepository.UpdateRate(id, rateRequest);
                return Ok(rate);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        // DELETE: api/rates/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var isDeleted = rateRepository.DeleteRate(id);
                return Ok(isDeleted);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}