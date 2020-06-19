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
        public ICollection<MovieDTO> GetAllMoviesComing(int day);

        public ICollection<MovieDTO> GetAllMoviesNowOn();

        public ICollection<MovieDTO> GetAllMovies(string query,int skip,int limit); 
        public Movie CreateMovie(MovieRequest movieRequest);

        public Movie GetMovieById(int id);

        public Movie UpdateMovie(int id, UpdateMovieRequest movieRequest);
        public bool DeleteMovie(Movie movie);
        public bool Save();
    }
}
