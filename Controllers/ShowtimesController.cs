using cinema_core.DTOs.ShowtimeDTOs;
using cinema_core.Form;
using cinema_core.Repositories;
using cinema_core.Repositories.Interfaces;
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
        private IMovieRepository movieRepository;
        private IScreenTypeRepository screenTypeRepository;
        private IRoomRepository roomRepository;

        public ShowtimesController(IShowtimeRepository showtimeRepository, IMovieRepository movieRepository,
                IScreenTypeRepository screenTypeRepository, IRoomRepository roomRepository)
        {
            this.showtimeRepository = showtimeRepository;
            this.movieRepository = movieRepository;
            this.screenTypeRepository = screenTypeRepository;
            this.roomRepository = roomRepository;
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

                bool isValidRequest = true;

                var movieSupportedScreenTypeIds = screenTypeRepository
                                                    .GetScreenTypesByMovieId(showtimeRequest.MovieId)
                                                    .Select(screenTypeDTO => screenTypeDTO.Id).ToList();
                var roomSupportedScreenTypeIds = screenTypeRepository
                                                    .GetScreenTypesByRoomId(showtimeRequest.RoomId)
                                                    .Select(screenTypeDTO => screenTypeDTO.Id).ToList();
                isValidRequest = isValidRequest && movieSupportedScreenTypeIds.Intersect(roomSupportedScreenTypeIds)
                                                    .Contains(showtimeRequest.ScreenTypeId);

                DateTime startAt = DateTime.Parse(showtimeRequest.StartAt);
                DateTime endAt = DateTime.Parse(showtimeRequest.EndAt);
                DateTime movieEndAt = movieRepository.GetMovieById(showtimeRequest.MovieId).EndAt;

                isValidRequest = isValidRequest && DateTime.Compare(endAt, movieEndAt) <= 0;
                isValidRequest = isValidRequest && DateTime.Compare(startAt, endAt) < 0;

                if (!isValidRequest)
                {
                    ModelState.AddModelError("", "Showtime went oopsie");
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
