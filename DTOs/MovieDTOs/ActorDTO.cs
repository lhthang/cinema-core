using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.DTOs.MovieDTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }

        public ActorDTO() { }
        public ActorDTO(Actor actor)
        {
            if (actor == null)
                return;

            this.Id = actor.Id;
            this.Name = actor.Name;
            this.Avatar = actor.Avatar;
        }
    }
}
