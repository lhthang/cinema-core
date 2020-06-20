using cinema_core.Form;
using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.DTOs.GenreDTOs
{
    public class GenreDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public GenreDTO(Genre genre)
        {
            if (genre == null)
                return;

            Id = genre.Id;
            Name = genre.Name;
            Description = genre.Description;
        }
    }
}
