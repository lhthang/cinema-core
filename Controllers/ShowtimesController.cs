using cinema_core.DTOs.ShowtimeDTOs;
using cinema_core.Form;
using cinema_core.Repositories;
using cinema_core.Utils.Error;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Controllers
{
    [Route("api/showtimes")]
    [ApiController]
    public class ShowtimesController : Controller
    {
        private IShowtimeRepository showtimeRepository;

        public ShowtimesController(IShowtimeRepository showtimeRepository)
        {
            this.showtimeRepository = showtimeRepository;
        }

        //GET: api/showtimes
        [HttpGet]
        public IActionResult Get(int skip = 0, int limit = 100000)
        {
            if (limit <= 0)
            {
                var error = new Error() { Message = "Limit must be greater than 0" };
                return StatusCode(400, error);
            }
            var showtimes = showtimeRepository.GetAllShowtimes(skip, limit);
            return Ok(showtimes);
        }

        // GET: api/showtimes/5
        [HttpGet("{id}", Name = "GetShowtime")]
        public IActionResult Get(int id)
        {
            var showtime = showtimeRepository.GetShowtimeById(id);
            if (showtime == null)
            {
                return NotFound();
            }
            var showtimeDTO = new ShowtimeDTO(showtime);
            return Ok(showtimeDTO);
        }

        // POST: api/showtimes
        [HttpPost]
        public IActionResult Post([FromBody] ShowtimeRequest showtimeRequest)
        {
            if (showtimeRequest == null)
            {
                return StatusCode(400, ModelState);
            }

            var statusCode = ValidateShowtime(showtimeRequest);


            if (!ModelState.IsValid)
            {
                return StatusCode(statusCode.StatusCode);
            }

            var showtime = showtimeRepository.CreateShowtime(showtimeRequest);
            if (showtime == null)
            {
                var error = new Error() { Message = "Showtime went oopsie when creating" };
                return StatusCode(400, error);
            }
            return CreatedAtRoute("GetShowtime", new { id = showtime.Id }, new ShowtimeDTO(showtime));
        }

        // POST: api/showtimes
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ShowtimeRequest showtimeRequest)
        {
            if (showtimeRepository.GetShowtimeById(id) == null)
            {
                return NotFound();
            }

            if (showtimeRequest == null)
            {
                return StatusCode(400, ModelState);
            }

            var statusCode = ValidateShowtime(showtimeRequest);

            if (!ModelState.IsValid)
            {
                return StatusCode(statusCode.StatusCode);
            }

            var showtime = showtimeRepository.UpdateShowtime(id, showtimeRequest);
            if (showtime == null)
            {
                var error = new Error() { Message = "Showtime went oopsie when updating" };
                return StatusCode(400, error);
            }
            return CreatedAtRoute("GetShowtime", new { id = showtime.Id }, new ShowtimeDTO(showtime));
        }

        // DELETE: api/showtimes/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var showtime = showtimeRepository.GetShowtimeById(id);
            if (showtime == null)
            {
                return NotFound();
            }

            if (!showtimeRepository.DeleteShowtime(showtime))
            {
                var error = new Error() { Message = "Showtime went oopsie when deleting" };
                return StatusCode(400, error);
            }
            return Ok(showtime);
        }

        private StatusCodeResult ValidateShowtime(ShowtimeRequest showtimeRequest)
        {
            // TODO: Check showtime.status here

            try 
            {
                DateTime startAt = DateTime.Parse(showtimeRequest.StartAt);
                DateTime endAt = DateTime.Parse(showtimeRequest.EndAt);
                if (DateTime.Compare(startAt, endAt) >= 0)
                {
                    ModelState.AddModelError("", "Movie showtime breaks space time continuum");
                    return StatusCode(400);
                }
            }
            catch (SystemException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return StatusCode(400);
            }

            return NoContent();
        }
    }
}
