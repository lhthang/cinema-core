using cinema_core.DTOs.ShowtimeDTOs;
using cinema_core.Form;
using cinema_core.Repositories;
using cinema_core.Repositories.Interfaces;
using cinema_core.Utils.ErrorHandle;
using cinema_core.Utils.Constants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cinema_core.Models;
using Microsoft.AspNetCore.Authorization;
using cinema_core.Utils;

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
        private ITicketRepository ticketRepository;

        public ShowtimesController(IShowtimeRepository showtimeRepository, IMovieRepository movieRepository,
                IScreenTypeRepository screenTypeRepository, IRoomRepository roomRepository, ITicketRepository ticket)
        {
            this.showtimeRepository = showtimeRepository;
            this.movieRepository = movieRepository;
            this.screenTypeRepository = screenTypeRepository;
            this.roomRepository = roomRepository;
            this.ticketRepository = ticket;
        }

        //GET: api/showtimes
        [HttpGet]
        public IActionResult Get(int skip = 0, int limit = 100000, int cluster = -1, int movie = -1)
        {
            if (limit <= 0)
            {
                var error = new Error() { message = "Limit must be greater than 0" };
                return StatusCode(400, error);
            }

            ICollection<ShowtimeDTO> showtimes;
            if (cluster != -1)
            {
                if (movie != -1)
                {
                    showtimes = showtimeRepository.GetShowtimesByClusterIdAndMovieId(cluster, movie);
                }
                else
                {
                    showtimes = showtimeRepository.GetShowtimesByClusterId(cluster);
                }
            }
            else
            {
                showtimes = showtimeRepository.GetAllShowtimes(skip, limit);
            }
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
            var tickets = ticketRepository.GetAllTicketsByShowtimeId(id);

            List<string> seats = new List<string>();
            foreach(var ticket in tickets)
            {
                seats.Add(ticket.Seat);
            }
            return Ok(new { showtime = showtimeDTO,seats = seats});
        }

        // POST: api/showtimes
        [HttpPost]
        [Authorize(Roles = Authorize.Admin + "," + Authorize.Staff)]
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
                var error = new Error() { message = "Showtime went oopsie when creating" };
                return StatusCode(400, error);
            }
            return RedirectToRoute("GetShowtime", new { id = showtime.Id });
        }

        // POST: api/showtimes
        [HttpPut("{id}")]
        [Authorize(Roles = Authorize.Admin + "," + Authorize.Staff)]
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
                var error = new Error() { message = "Showtime went oopsie when updating" };
                return StatusCode(400, error);
            }
            return Ok(new ShowtimeDTO(showtime));
        }

        // DELETE: api/showtimes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Authorize.Admin + "," + Authorize.Staff)]
        public IActionResult Delete(int id)
        {
            var showtime = showtimeRepository.GetShowtimeById(id);
            if (showtime == null)
            {
                return NotFound();
            }

            if (!showtimeRepository.DeleteShowtime(showtime))
            {
                var error = new Error() { message = "Showtime went oopsie when deleting" };
                return StatusCode(400, error);
            }
            return Ok(showtime);
        }

        private StatusCodeResult ValidateShowtime(ShowtimeRequest showtimeRequest)
        {
            try
            {

                bool validFlag;

                // Check Status
                validFlag = showtimeRequest.Status == Constants.SHOWTIME_STATUS_ACTIVE 
                                    || showtimeRequest.Status == Constants.SHOWTIME_STATUS_INACTIVE ;
                if (!validFlag)
                {
                    ModelState.AddModelError("", $"Status not valid, can only be {Constants.SHOWTIME_STATUS_ACTIVE} "
                            + $"or {Constants.SHOWTIME_STATUS_INACTIVE}");
                    return StatusCode(400);
                }

                // Check RoomId, ScreenTypeId and MovieId valid
                var movieSupportedScreenTypeIds = screenTypeRepository
                                                    .GetScreenTypesByMovieId(showtimeRequest.MovieId)
                                                    .Select(screenTypeDTO => screenTypeDTO.Id).ToList();
                var roomSupportedScreenTypeIds = screenTypeRepository
                                                    .GetScreenTypesByRoomId(showtimeRequest.RoomId)
                                                    .Select(screenTypeDTO => screenTypeDTO.Id).ToList();
                validFlag = movieSupportedScreenTypeIds.Intersect(roomSupportedScreenTypeIds)
                                                    .Contains(showtimeRequest.ScreenTypeId);
                if (!validFlag)
                {
                    ModelState.AddModelError("", "Unsupported ScreenType for Room or Movie");
                    return StatusCode(400);
                }

                // Check end at
                DateTime startAt = DateTime.Parse(showtimeRequest.StartAt);
                Movie movie = movieRepository.GetMovieById(showtimeRequest.MovieId);
                DateTime endAt = startAt.AddMinutes(movie.Runtime);
                DateTime movieEndAt = movie.EndAt;
                validFlag = DateTime.Compare(endAt, movieEndAt) <= 0;
                if (!validFlag)
                {
                    ModelState.AddModelError("", "Showtime breaks space time continuum");
                    return StatusCode(400);
                }
                DateTime now = DateTime.Now;
                validFlag = DateTime.Compare(now, endAt) <= 0;
                if (!validFlag)
                {
                    ModelState.AddModelError("", "Need a time machine to see this movie");
                    return StatusCode(400);
                }

                // Check showtime overlap
                var showtimes = showtimeRepository.GetShowtimesByRoomId(showtimeRequest.RoomId);
                foreach (ShowtimeDTO showtime in showtimes)
                {
                    validFlag = showtime.StartAt > endAt || showtime.EndAt < startAt;
                    if (!validFlag)
                    {
                        ModelState.AddModelError("", "Showtimes overlap");
                        return StatusCode(400);
                    }
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
