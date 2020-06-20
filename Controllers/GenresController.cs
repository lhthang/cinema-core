using cinema_core.DTOs.GenreDTOs;
using cinema_core.Form;
using cinema_core.Repositories.Interfaces;
using cinema_core.Utils.Error;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenresController : Controller
    {
        private IGenreRepository genreRepository;

        public GenresController(IGenreRepository repository)
        {
            genreRepository = repository;
        }
        // GET: api/genres
        [HttpGet]
        public IActionResult Get(int skip = 0, int limit = 100000)
        {
            if (limit <= 0)
            {
                var error = new Error() { Message = "Limit must be greater than 0" };
                return StatusCode(400, error);
            }
            var genres = genreRepository.GetAllGenres(skip, limit);
            return Ok(genres);
        }

        private IActionResult StatusCode(int v, Error error)
        {
            throw new Exception(error.Message);
        }

        // GET: api/genres/5
        [HttpGet("{id}", Name = "GetGenre")]
        public IActionResult Get(int id)
        {
            var genre = genreRepository.GetGenreById(id);
            if (genre == null)
                return NotFound();

            return Ok(genre);
        }

        // POST: api/genres
        [HttpPost]
        public IActionResult Post([FromBody] GenreRequest genreRequest)
        {
            if (genreRequest == null) return StatusCode(400, ModelState);

            var statusCode = ValidateGenre(genreRequest);


            if (!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);

            var genre = genreRepository.CreateGenre(genreRequest);
            if (genre == null)
            {
                var error = new Error() { Message = "Something went wrong when save genre" };
                return StatusCode(400, error);
            }
            return RedirectToRoute("GetGenre", new { id = genre.Id });
        }

        // POST: api/genres
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] GenreRequest genreRequest)
        {
            if (genreRequest == null) return StatusCode(400, ModelState);

            var statusCode = ValidateGenre(genreRequest);

            if (!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);

            var genre = genreRepository.UpdateGenre(id, genreRequest);
            if (genre == null)
            {
                var error = new Error() { Message = "Something went wrong when save genre" };
                return StatusCode(400, error);
            }
            return RedirectToRoute("GetGenre", new { id = genre.Id });
        }

        // DELETE: api/genre/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //var isExist = genreRepository.GetGenreById(id);
            //if (isExist == null) return NotFound();

            if (!genreRepository.DeleteGenre(id))
            {
                var error = new Error() { Message = "Something went wrong when delete genre" };
                return StatusCode(400, error);
            }
            return Ok(id);
        }

        private StatusCodeResult ValidateGenre(GenreRequest genreRequest)
        {
            if (genreRequest == null || !ModelState.IsValid) return BadRequest();

            return NoContent();
        }
    }
}
