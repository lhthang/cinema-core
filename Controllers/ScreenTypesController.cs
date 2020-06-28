using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Repositories;
using cinema_core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

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
        public IActionResult Get()
        {
            try
            {
                var screenTypes = screenTypeRepository.GetScreenTypes();
                return Ok(screenTypes);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        // GET: api/screen-types/GetScreenTypesByMovieId/1
        [HttpGet("[action]/{movieId}", Name = "GetScreenTypesByMovieId")]
        [AllowAnonymous]
        public IActionResult GetScreenTypesByMovieId(int movieId)
        {
            try
            {
                var screenTypes = screenTypeRepository.GetScreenTypesByMovieId(movieId);
                return Ok(screenTypes);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        // GET: api/screen-types/GetScreenTypesByRoomId/1
        [HttpGet("[action]/{roomId}", Name = "GetScreenTypesByRoomId")]
        public IActionResult GetScreenTypesByRoomId(int roomId)
        {
            try
            {
                var screenTypes = screenTypeRepository.GetScreenTypesByRoomId(roomId);
                return Ok(screenTypes);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // GET: api/screen-types/5
        [HttpGet("{id}", Name = "GetScreenType")]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            try
            {
                var screenType = screenTypeRepository.GetScreenTypeById(id);
                return Ok(screenType);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // POST: api/screen-types
        [HttpPost]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Post([FromBody] ScreenTypeRequest screenTypeRequest)
        {
            if (screenTypeRequest == null)
                return StatusCode(400, ModelState);
            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            try
            {
                var screenType = screenTypeRepository.CreateScreenType(screenTypeRequest);
                return Ok(screenType);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // PUT: api/screen-types/5
        [HttpPut("{id}")]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Put(int id, [FromBody] ScreenTypeRequest screenTypeRequest)
        {
            if (screenTypeRequest == null)
                return StatusCode(400, ModelState);
            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            try
            {
                var screenType = screenTypeRepository.UpdateScreenType(id, screenTypeRequest);
                return Ok(screenType);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // DELETE: api/screen-types/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Delete(int id)
        {
            try
            {
                var isDeleted = screenTypeRepository.DeleteScreenType(id);
                return Ok(isDeleted);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
