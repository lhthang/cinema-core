using cinema_core.DTOs.MovieDTOs;
using cinema_core.Form;
using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Interfaces
{
    public interface IActorRepository
    {
        public ICollection<ActorDTO> GetActors(int skip, int limit);
        public ActorDTO GetActorById(int Id);
        public Actor GetActorByName(string name);
        public ActorDTO CreateActor(ActorRequest actorRequest);
        public ActorDTO UpdateActor(int id, ActorRequest actorRequest);
        bool DeleteActor(int id);
        //public ICollection<ActorDTO> GetAllActors(int skip,int limit);
        //public Actor UpdateActor(int id,Actor actor);

        //public Actor GetActorById(int id);

        //public Actor AddActor(Actor actor);


        //public Actor DeleteActorById(int id);

        //public bool Save();
    }
}
