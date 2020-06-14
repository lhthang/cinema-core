using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cinema_core.DTOs.MovieDTOs;
using cinema_core.Form;
using cinema_core.Repositories;
using cinema_core.Repositories.Interfaces;
using cinema_core.Utils.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cinema_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : Controller
    {
        private IMovieRepository movieRepository;
        private IScreenTypeRepository screenTypeRepository;

        public MoviesController(IMovieRepository repository,IScreenTypeRepository screenTypeRepository)
        {
            this.screenTypeRepository = screenTypeRepository;
            this.movieRepository = repository;
        }

        // GET: api/movies/now-on
        [HttpGet("[action]")]
        public IActionResult GetAllMoviesNowOn()
        {
            var movie = movieRepository.GetAllMoviesNowOn();
            return Ok(movie);
        }

        // GET: api/rooms/5
        [HttpGet("{id}", Name = "GetMovie")]
        public IActionResult Get(int id)
        {
            var movie = movieRepository.GetMovieById(id);
            if (movie == null)
                return NotFound();
            var movieDTO = new MovieDTO(movie);
            return Ok(movieDTO);
        }


        // POST: api/rooms
        [HttpPost]
        public IActionResult Post([FromBody] MovieRequest movieRequest)
        {
            if (movieRequest == null) return StatusCode(400, ModelState);

            var statusCode = ValidateMovie(movieRequest);


            if (!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode,ModelState);

            var movie = movieRepository.CreateMovie(movieRequest);
            if (movie == null)
            {
                var error = new Error() { Message = "Something went wrong when save movie" };
                return StatusCode(400, error);
            }
            return RedirectToRoute("GetMovie", new { id = movie.Id });
        }

        private StatusCodeResult ValidateMovie(MovieRequest movieRequest)
        {
            if (movieRequest == null || !ModelState.IsValid) return BadRequest();

            foreach (var id in movieRequest.ScreenTypeIds)
            {
                if (screenTypeRepository.GetScreenTypeById(id) == null)
                {
                    ModelState.AddModelError("", $"Id {id} not found");
                    return StatusCode(404);
                }
            }
            return NoContent();
        }
    }
}