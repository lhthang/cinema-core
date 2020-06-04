using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cinema_core.DTOs.ScreenTypeDTO;
using cinema_core.Models;
using cinema_core.Models.User;
using cinema_core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cinema_core.Controllers
{
    [Route("api/screen-types")]
    [ApiController]
    public class ScreenTypesController : Controller
    {
        private IScreenTypeRepository screenTypeRepository;

        public ScreenTypesController(IScreenTypeRepository repository)
        {
            screenTypeRepository = repository;
        }
        // GET: api/screen-types
        [HttpGet]
        //[Authorize(Policy =Policies.Admin)]
        [Authorize()]
        public IActionResult Get()
        {
            var screenTypes = screenTypeRepository.GetScreenTypes();
            
            return Ok(screenTypes);
        }

        // GET: api/screen-types/5
        [HttpGet("{id}", Name = "Get")]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            var screenType = screenTypeRepository.GetScreenTypeById(id);
            if (screenType == null)
                return NotFound();
            return Ok(screenType);
        }

        // POST: api/screen-types
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/screen-types/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/screen-types/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
