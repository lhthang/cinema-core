using cinema_core.DTOs.GenreDTOs;
using cinema_core.Form;
using cinema_core.Repositories.Interfaces;
using cinema_core.Utils;
using cinema_core.Utils.ErrorHandle;
using Microsoft.AspNetCore.Authorization;
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
            try
            {
                var genres = genreRepository.GetAllGenres(skip, limit);
                return Ok(genres);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // GET: api/genres/5
        [HttpGet("{id}", Name = "GetGenre")]
        public IActionResult Get(int id)
        {
            try
            {
                var genre = genreRepository.GetGenreById(id);
                return Ok(genre);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // POST: api/genres
        [HttpPost]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Post([FromBody] GenreRequest genreRequest)
        {
            if (genreRequest == null)
                return StatusCode(400, ModelState);
            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            try
            {
                var genre = genreRepository.CreateGenre(genreRequest);
                return Ok(genre);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // PUT: api/genres/5
        [HttpPut("{id}")]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Put(int id, [FromBody] GenreRequest genreRequest)
        {
            if (genreRequest == null)
                return StatusCode(400, ModelState);
            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            try
            {
                var genre = genreRepository.UpdateGenre(id, genreRequest);
                return Ok(genre);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // DELETE: api/genres/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Delete(int id)
        {
            try
            {
                var isDeleted = genreRepository.DeleteGenre(id);
                return Ok(isDeleted);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
