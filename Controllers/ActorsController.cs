using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using cinema_core.DTOs.MovieDTOs;
using cinema_core.ErrorHandle;
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

        [HttpDelete("{id}")]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Delete(int id)
        {
            try
            {
                var actor = actorRepository.DeleteActorById(id);
                return Ok(new ActorDTO(actor));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles =Authorize.Admin)]
        public IActionResult Put(int id, [FromBody] Actor actor)
        {
            try
            {
                var isExist = actorRepository.GetActorById(id);


                if (!ModelState.IsValid)
                {
                    string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                    throw new CustomException(HttpStatusCode.BadRequest, messages);
                }
                    

                var updatedActor = actorRepository.UpdateActor(id, actor);

                if (updatedActor == null)
                {
                    ModelState.AddModelError("", "Something went wrong when update actor");
                    string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                    throw new CustomException(HttpStatusCode.BadRequest, messages);
                }
                return Ok(new ActorDTO(actor));
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        [HttpPost("")]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Post([FromBody] Actor actor)
        {
            try
            {
                var isExist = actorRepository.GetActorByName(actor.Name);

                if (!ModelState.IsValid)
                {
                    string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                    throw new CustomException(HttpStatusCode.BadRequest, messages);
                }


                var result = actorRepository.AddActor(actor);
                return Ok(new ActorDTO(result));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}