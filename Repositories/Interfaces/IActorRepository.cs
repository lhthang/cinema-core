using cinema_core.DTOs.MovieDTOs;
using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Interfaces
{
    public interface IActorRepository
    {
        public ICollection<ActorDTO> GetAllActors(int skip,int limit);
        public Actor UpdateActor(int id,Actor actor);

        public Actor GetActorById(int id);

        public bool Save();
    }
}
