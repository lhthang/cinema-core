using cinema_core.DTOs.MovieDTOs;
using cinema_core.Models;
using cinema_core.Models.Base;
using cinema_core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class ActorRepository : IActorRepository
    {
        private MyDbContext dbContext;

        public ActorRepository(MyDbContext context)
        {
            dbContext = context;
        }

        public Actor GetActorById(int id)
        {
            var actor = dbContext.Actors.Where(a => a.Id == id).FirstOrDefault();
            return actor;
        }

        public ICollection<ActorDTO> GetAllActors(int skip, int limit)
        {
            var actors = dbContext.Actors.OrderBy(a => a.Name).Skip(skip).Take(limit).ToList();
            List<ActorDTO> actorDTOs = new List<ActorDTO>();
            foreach (var actor in actors)
            {
                actorDTOs.Add(new ActorDTO(actor));
            }
            return actorDTOs;
        }

        public bool Save()
        {
            return dbContext.SaveChanges() > 0;
        }

        public Actor UpdateActor(int id,Actor actor)
        {
            var actorInDb = dbContext.Actors.Where(a => a.Id == id).FirstOrDefault();
            actorInDb.Avatar = actor.Avatar;
            dbContext.Update(actorInDb);
            var isSuccess = Save();
            if (!isSuccess) return null;
            return actorInDb;
        }
    }
}
