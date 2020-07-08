using cinema_core.Form;
using cinema_core.Repositories.Interfaces;
using cinema_core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PromotionsController : Controller
	{
        private IPromotionRepository promotionRepository;

        public PromotionsController(IPromotionRepository repository)
        {
            promotionRepository = repository;
        }

        // GET: api/promotions
        [HttpGet]
        public IActionResult Get(int skip = 0, int limit = 100000)
        {
            try
            {
                var promotions = promotionRepository.GetPromotions(skip, limit);
                return Ok(promotions);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // GET: api/promotions/5
        [HttpGet("{id}", Name = "GetPromotion")]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            try
            {
                var promotion = promotionRepository.GetPromotionById(id);
                return Ok(promotion);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // GET: api/promotions
        [HttpGet("[action]")]
        public IActionResult CheckPromotion(string promotionCode)
        {
            try
            {
                var promotion = promotionRepository.CheckPromotion(promotionCode);
                return Ok(promotion);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // POST: api/promotions
        [HttpPost]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Post([FromBody] PromotionRequest promotionRequest)
        {
            if (promotionRequest == null)
                return StatusCode(400, ModelState);
            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            try
            {
                var promotion = promotionRepository.CreatePromotion(promotionRequest);
                return Ok(promotion);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // PUT: api/promotions/5
        [HttpPut("{id}")]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Put(int id, [FromBody] PromotionRequest promotionRequest)
        {
            if (promotionRequest == null)
                return StatusCode(400, ModelState);
            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            try
            {
                var promotion = promotionRepository.UpdatePromotion(id, promotionRequest);
                return Ok(promotion);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // DELETE: api/promotions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Delete(int id)
        {
            try
            {
                var isDeleted = promotionRepository.DeletePromotion(id);
                return Ok(isDeleted);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
