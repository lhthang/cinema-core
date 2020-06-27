using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cinema_core.DTOs.MovieDTOs;
using cinema_core.Models;
using cinema_core.Repositories.Interfaces;
using cinema_core.Utils.ErrorHandle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cinema_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : Controller
    {
        private IActorRepository actorRepository;

        public ActorsController(IActorRepository repository)
        {
            actorRepository = repository;
        }

        [HttpGet]
        public IActionResult Get(int skip = 0, int limit = 100000)
        {
            if (limit <= 0)
            {
                var error = new Error() { message = "Limit must be greater than 0" };
                return StatusCode(400, error);
            }
            var actors = actorRepository.GetAllActors(skip,limit);
            return Ok(actors);
        }

        [HttpGet("{id}",Name ="GetActor")]
        public IActionResult Get(int id)
        {
            var actor = actorRepository.GetActorById(id);
            if (actor == null) return NotFound();

            return Ok(new ActorDTO(actor));
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Actor actor)
        {
            var isExist = actorRepository.GetActorById(id);
            if (isExist == null) return NotFound();

            if (actor == null) return StatusCode(400, ModelState);


            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            var updatedActor = actorRepository.UpdateActor(id,actor);

            if (updatedActor==null)
            {
                ModelState.AddModelError("", "Something went wrong when update actor");
                return StatusCode(400, ModelState);
            }
            return Ok(new ActorDTO(actor));
        }
    }
}