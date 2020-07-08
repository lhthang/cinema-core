using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using cinema_core.DTOs.MovieDTOs;
using cinema_core.ErrorHandle;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Repositories.Interfaces;
using cinema_core.Utils;
using cinema_core.Utils.ErrorHandle;
using Microsoft.AspNetCore.Authorization;
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

        // GET: api/actors
        [HttpGet]
        public IActionResult Get(int skip = 0, int limit = 100000)
        {
            try
            {
                var actors = actorRepository.GetActors(skip, limit);
                return Ok(actors);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // GET: api/actors/5
        [HttpGet("{id}", Name = "GetActor")]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            try
            {
                var actor = actorRepository.GetActorById(id);
                return Ok(actor);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // POST: api/actors
        [HttpPost]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Post([FromBody] ActorRequest actorRequest)
        {
            if (actorRequest == null)
                return StatusCode(400, ModelState);
            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            try
            {
                var actor = actorRepository.CreateActor(actorRequest);
                return Ok(actor);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // PUT: api/actors/5
        [HttpPut("{id}")]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Put(int id, [FromBody] ActorRequest actorRequest)
        {
            if (actorRequest == null)
                return StatusCode(400, ModelState);
            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            try
            {
                var actor = actorRepository.UpdateActor(id, actorRequest);
                return Ok(actor);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // DELETE: api/actors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Delete(int id)
        {
            try
            {
                var isDeleted = actorRepository.DeleteActor(id);
                return Ok(isDeleted);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}