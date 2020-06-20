using cinema_core.DTOs.GenreDTOs;
using cinema_core.Form;
using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        public ICollection<GenreDTO> GetAllGenres(int skip, int limit);
        public GenreDTO GetGenreById(int id);
        public GenreDTO GetGenreByName(string name);
        public GenreDTO CreateGenre(GenreRequest genreRequest);
        public GenreDTO UpdateGenre(int id, GenreRequest genreRequest);
        public bool DeleteGenre(int id);
    }
}
