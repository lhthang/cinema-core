using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cinema_core.DTOs.MovieDTOs;
using cinema_core.Form;
using cinema_core.Models;
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
        private IRateRepository rateRepository;
        private IGenreRepository genreRepository;

        public MoviesController(IMovieRepository repository,IScreenTypeRepository screenTypeRepository, 
            IRateRepository rateRepository,IGenreRepository genreRepository)
        {
            this.screenTypeRepository = screenTypeRepository;
            this.movieRepository = repository;
            this.rateRepository = rateRepository;
            this.genreRepository = genreRepository;
        }

        // GET: api/movies/now-on
        [HttpGet]
        public IActionResult Get(string query = "", int skip =0,int limit =10000)
        {
            if (limit <= 0)
            {
                Error error = new Error() { Message = "Limit must be greater than 0" };
                return StatusCode(400, error);
            }
            var movie = movieRepository.GetAllMovies(query,skip,limit);
            return Ok(movie);
        }

        // GET: api/movies/now-on
        [HttpGet("[action]")]
        public IActionResult GetAllMoviesNowOn()
        {
            var movie = movieRepository.GetAllMoviesNowOn();
            return Ok(movie);
        }

        // GET: api/movies/now-on
        [HttpGet("[action]")]
        public IActionResult GetAllMoviesComing(int day=30)
        {
            if (day <= 0)
            {
                Error error = new Error() { Message = "Query coming day must be greater than 0" };
                return StatusCode(400, error);
            }
            var movie = movieRepository.GetAllMoviesComing(day);
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

        // POST: api/rooms
        [HttpPut("{id}")]
        public IActionResult Put(int id,[FromBody] UpdateMovieRequest movieRequest)
        {
            if (movieRepository.GetMovieById(id) == null) return NotFound();

            if (movieRequest == null) return StatusCode(400, ModelState);

            var statusCode = ValidateUpdateMovie(movieRequest);


            if (!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode, ModelState);

            var movie = movieRepository.UpdateMovie(id,movieRequest);
            if (movie == null)
            {
                var error = new Error() { Message = "Something went wrong when save movie" };
                return StatusCode(400, error);
            }
            return Ok(new MovieDTO(movie));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var isExist = movieRepository.GetMovieById(id);
            if (isExist == null) return NotFound();

            if (!movieRepository.DeleteMovie(isExist))
            {
                var error = new Error() { Message = "Something went wrong when save movie" };
                return StatusCode(400, error);
            }
            return Ok(new MovieDTO(isExist));
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

            if (rateRepository.GetRateById(movieRequest.RateId) == null)
            {
                ModelState.AddModelError("", $"Rate id {movieRequest.RateId} not found");
                return StatusCode(404);
            }
            return NoContent();
        }

        private StatusCodeResult ValidateUpdateMovie(UpdateMovieRequest movieRequest)
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

            if (movieRequest.GenreIds != null)
            {
                foreach (var id in movieRequest.GenreIds)
                {
                    if (genreRepository.GetGenreById(id) == null)
                    {
                        ModelState.AddModelError("", $"Genre {id} not found");
                        return StatusCode(404);
                    }
                }
            }

            if (rateRepository.GetRateById(movieRequest.RateId) == null)
            {
                ModelState.AddModelError("", $"Rate id {movieRequest.RateId} not found");
                return StatusCode(404);
            }
            return NoContent();
        }
    }
}