using cinema_core.DTOs.MovieDTOs;
using cinema_core.Form;
using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        public ICollection<MovieDTO> GetAllMoviesNowOn();
        public Movie CreateMovie(MovieRequest movieRequest);

        public Movie GetMovieById(int id);
        public bool Save();
    }
}
