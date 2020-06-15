using cinema_core.DTOs.MovieDTOs;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Models.Base;
using cinema_core.Repositories.Interfaces;
using cinema_core.Utils.MovieProxy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class MovieRepository : IMovieRepository
    {
        private MyDbContext dbContext;

        public MovieRepository(MyDbContext context)
        {
            dbContext = context;
        }
        public Movie CreateMovie(MovieRequest movieRequest)
        {
            MovieResponse response = MovieProxy.GetMovieByIMDB(movieRequest.Imdb);
            var movie = new Movie()
            {
                Title = response.Title,
                Country = response.Country,
                Languages = response.Languages.ToArray(),
                Runtime = response.Runtime,
                Directors = response.Directors.ToArray(),
                ReleasedAt = response.ReleasedAt,
                Poster = response.Poster,
                EndAt = DateTime.Parse(movieRequest.EndAt),
                Wallpapers = movieRequest.Wallpapers.ToArray(),
                Trailer = movieRequest.Trailer,
                Story = movieRequest.Story,
            };

            var screenTypes = dbContext.ScreenTypes.Where(s => movieRequest.ScreenTypeIds.Contains(s.Id)).ToList();

            List<string> actorNames = new List<string>();
            foreach(var actor in response.Actors)
            {
                actorNames.Add(actor.Name);
            }

            var actors = dbContext.Actors.Where(a => actorNames.Contains(a.Name)).ToList();

            //var isNot = movieRequest.Actors.Where(a => !actors.Any(a2=>a2.Name==a.Name)).ToList();

            var isNotExistActors = response.Actors.Where(a => !actors.Any(a2 => a2.Name == a.Name)).ToList();
            
            if (isNotExistActors != null)
            {
                foreach (var actor in isNotExistActors)
                {
                    dbContext.Add(actor);
                }
            }

            foreach (var screenType in screenTypes)
            {
                var movieScreenType = new MovieScreenType()
                {
                    Movie = movie,
                    ScreenType = screenType,
                };
                dbContext.Add(movieScreenType);
            }

            foreach (var actor in actors.Concat(isNotExistActors))
            {
                var movieActor = new MovieActor()
                {
                    Movie = movie,
                    Actor = actor,
                };
                dbContext.Add(movieActor);
            }

            dbContext.Add(movie);
            var isSuccess = Save();
            if (!isSuccess) return null;
            return movie;
        }

        public bool DeleteMovie(Movie movie)
        {
            dbContext.Remove(movie);
            return Save();
        }

        public ICollection<MovieDTO> GetAllMovies()
        {
            var movies = dbContext.Movies.OrderByDescending(m=>m.ReleasedAt)
                .Include(ms => ms.MovieScreenTypes).ThenInclude(s => s.ScreenType)
                .Include(ma => ma.MovieActors).ThenInclude(a => a.Actor).ToList();

            List<MovieDTO> movieDTOs = new List<MovieDTO>();
            foreach (var movie in movies)
            {
                movieDTOs.Add(new MovieDTO(movie));
            }
            return movieDTOs;
        }

        public ICollection<MovieDTO> GetAllMoviesComing(int day)
        {
            var movies = dbContext.Movies.Where(m => DateTime.Compare(m.ReleasedAt, DateTime.Now) >=0 && DateTime.Compare(m.ReleasedAt, DateTime.Now.AddDays(day)) <= 0)
                .Include(ms => ms.MovieScreenTypes).ThenInclude(s => s.ScreenType)
                .Include(ma => ma.MovieActors).ThenInclude(a => a.Actor).OrderBy(m => m.ReleasedAt).ToList();

            List<MovieDTO> movieDTOs = new List<MovieDTO>();
            foreach (var movie in movies)
            {
                movieDTOs.Add(new MovieDTO(movie));
            }
            return movieDTOs;
        }

        public ICollection<MovieDTO> GetAllMoviesNowOn()
        {
            var movies = dbContext.Movies.Where(m => DateTime.Compare(m.ReleasedAt, DateTime.Now) <= 0 && DateTime.Compare(m.EndAt, DateTime.Now) >= 0)
                .Include(ms => ms.MovieScreenTypes).ThenInclude(s => s.ScreenType)
                .Include(ma => ma.MovieActors).ThenInclude(a => a.Actor).OrderBy(m => m.ReleasedAt).ToList();

            List<MovieDTO> movieDTOs = new List<MovieDTO>();
            foreach (var movie in movies)
            {
                movieDTOs.Add(new MovieDTO(movie));
            }
            return movieDTOs;
        }

        public Movie GetMovieById(int id)
        {
            var movie = dbContext.Movies.Where(m => m.Id == id).Include(ms =>  ms.MovieScreenTypes).ThenInclude(s => s.ScreenType)
                .Include(ma=>ma.MovieActors).ThenInclude(a=>a.Actor).FirstOrDefault();
            return movie;
        }

        public bool Save()
        {
            return dbContext.SaveChanges() > 0;
        }

        public Movie UpdateMovie(int id, UpdateMovieRequest movieRequest)
        {
            var movie = dbContext.Movies.Where(m => m.Id == id).FirstOrDefault();
            movie.Title = movieRequest.Title;
            movie.Poster = movieRequest.Poster;
            movie.EndAt = DateTime.Parse(movieRequest.EndAt);
            movie.Wallpapers = movieRequest.Wallpapers.ToArray();
            movie.Trailer = movieRequest.Trailer;
            movie.Story = movieRequest.Story;

            var screenTypeToDelete = dbContext.MovieScreenTypes.Where(ms => ms.MovieId == id).ToList();
            if (screenTypeToDelete != null)
                dbContext.RemoveRange(screenTypeToDelete);

            var screenTypes = dbContext.ScreenTypes.Where(s => movieRequest.ScreenTypeIds.Contains(s.Id)).ToList();

            foreach (var screenType in screenTypes)
            {
                var movieScreenType = new MovieScreenType()
                {
                    Movie = movie,
                    ScreenType = screenType,
                };
                dbContext.Add(movieScreenType);
            }

            dbContext.Update(movie);
            var isSuccess = Save();
            if (!isSuccess) return null;
            return movie;
        }
    }
}
