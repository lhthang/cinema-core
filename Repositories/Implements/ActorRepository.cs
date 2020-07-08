using cinema_core.DTOs.MovieDTOs;
using cinema_core.ErrorHandle;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Models.Base;
using cinema_core.Repositories.Base;
using cinema_core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class ActorRepository : BaseRepository, IActorRepository
    {
        public ActorRepository(MyDbContext context) : base(context)
        {
        }

        public ActorDTO GetActorById(int id)
        {
            var actor = GetActorEntityById(id);
            return new ActorDTO(actor);
        }

        public ICollection<ActorDTO> GetActors(int skip, int limit)
        {
            List<ActorDTO> results = new List<ActorDTO>();
            var actors = dbContext.Actors.OrderBy(sc => sc.Id).Skip(skip).Take(limit).ToList();
            foreach (Actor actor in actors)
            {
                results.Add(new ActorDTO(actor));
            }
            return results;
        }

        public Actor GetActorByName(string name)
        {
            throw new NotImplementedException();
        }

        public ActorDTO CreateActor(ActorRequest actorRequest)
        {
            var actor = new Actor()
            {
                Name = actorRequest.Name,
                Avatar = actorRequest.Avatar,
            };

            dbContext.Add(actor);
            Save();
            return new ActorDTO(actor);
        }

        public ActorDTO UpdateActor(int id, ActorRequest actorRequest)
        {
            var actor = GetActorEntityById(id);

            actor.Name = actorRequest.Name;
            actor.Avatar = actorRequest.Avatar;

            dbContext.Update(actor);
            Save();
            return new ActorDTO(actor);
        }

        public bool DeleteActor(int id)
        {
            var actorToDelete = GetActorEntityById(id);

            dbContext.Remove(actorToDelete);
            Save();
            return true;
        }

        private Actor GetActorEntityById(int id)
        {
            var actor = dbContext.Actors.Where(r => r.Id == id).FirstOrDefault();
            if (actor == null)
                throw new Exception("Id not found.");

            return actor;
        }

    }
}
