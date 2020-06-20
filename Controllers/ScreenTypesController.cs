using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Repositories;
using cinema_core.Utils;
using cinema_core.Utils.Error;
using cinema_core.Utils.MovieProxy;
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
        [Authorize(Roles=Authorize.Admin)]
        public IActionResult Get()
        {
            var screenTypes = screenTypeRepository.GetScreenTypes();
            return Ok(screenTypes);
        }

        // GET: api/screen-types/GetScreenTypesByMovieId/1
        [HttpGet("[action]/{movieId}", Name = "GetScreenTypesByMovieId")]
        [AllowAnonymous]
        public IActionResult GetScreenTypesByMovieId(int movieId)
        {
            var screenTypes = screenTypeRepository.GetScreenTypesByMovieId(movieId);
            if (screenTypes == null)
            {
                return NotFound();
            }
            return Ok(screenTypes);
        }

        // GET: api/screen-types/GetScreenTypesByRoomId/1
        [HttpGet("[action]/{roomId}", Name = "GetScreenTypesByRoomId")]
        [AllowAnonymous]
        public IActionResult GetScreenTypesByRoomId(int roomId)
        {
            var screenTypes = screenTypeRepository.GetScreenTypesByRoomId(roomId);
            if (screenTypes == null)
            {
                return NotFound();
            }
            return Ok(screenTypes);
        }

        // GET: api/screen-types/5
        [HttpGet("{id}", Name = "GetScreenType")]
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
        public IActionResult Post([FromBody] ScreenTypeRequest screenTypeRequest)
        {
            if (screenTypeRequest == null) return StatusCode(400, ModelState);

            var isExist = screenTypeRepository.GetScreenTypeByName(screenTypeRequest.Name);

            if (isExist != null)
            {
                Error error = new Error() { Message = $"Screen type {screenTypeRequest.Name} exists" };
                return StatusCode(400, error);
            }

            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            var screenType = screenTypeRepository.CreateScreenType(screenTypeRequest);
            if (screenType == null)
            {
                Error error = new Error() { Message = "Something went wrong when save screen type" };
                return StatusCode(400, error);
            }

            return Ok(screenType);
            //return RedirectToRoute("GetScreenType", new { id = screenType.Id });
        }

        // PUT: api/screen-types/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ScreenTypeRequest screenTypeRequest)
        {
            var isExist = screenTypeRepository.GetScreenTypeById(id);
            if (isExist == null) return NotFound();

            if (screenTypeRequest == null) return StatusCode(400, ModelState);

            var isDuplicate = screenTypeRepository.GetScreenTypeByName(screenTypeRequest.Name);

            if (isDuplicate != null)
            {
                Error error = new Error() { Message = $"Screen type {screenTypeRequest.Name} exists" };
                return StatusCode(400, error);
            }

            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            var screenType = screenTypeRepository.UpdateScreenType(id, screenTypeRequest);
            if (screenType == null)
            {
                Error error = new Error() { Message = "Something went wrong when update screen type" };
                return StatusCode(400, error);
            }

            return Ok(screenType);
            //return RedirectToRoute("GetScreenType", new { id = screenType.Id });
        }

        // DELETE: api/screen-types/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var isExist = screenTypeRepository.GetScreenTypeById(id);
            if (isExist == null) return NotFound();

            if (!screenTypeRepository.DeleteScreenType(id))
            {
                ModelState.AddModelError("", "Something went wrong when delete screen type");
                return StatusCode(400, ModelState);
            }
            return Ok(isExist);
        }
    }
}
