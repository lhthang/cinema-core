using cinema_core.DTOs.GenreDTOs;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Models.Base;
using cinema_core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class GenreRepository : IGenreRepository
    {
        private MyDbContext dbContext;

        public GenreRepository(MyDbContext context)
        {
            dbContext = context;
        }

        private bool Save()
        {
            return dbContext.SaveChanges() > 0;
        }

        public ICollection<GenreDTO> GetAllGenres(int skip, int limit)
        {
            List<Genre> genres = dbContext.Genres.OrderBy(r => r.Id).Skip(skip).Take(limit).ToList();
            return genres.Select(x => new GenreDTO(x)).ToList();
        }

        public GenreDTO GetGenreById(int id)
        {
            var genre = dbContext.Genres.Where(r => r.Id == id).Include(rs => rs.MovieGenres).ThenInclude(s => s.Movie).FirstOrDefault();
            return new GenreDTO(genre);
        }

        public GenreDTO CreateGenre(GenreRequest genreRequest)
        {
            var genre = new Genre()
            {
                Name = genreRequest.Name,
                Description = genreRequest.Description,
            };

            dbContext.Add(genre);
            var isSuccess = Save();
            if (!isSuccess) return null;
            return new GenreDTO(genre);
        }

        public GenreDTO UpdateGenre(int id, GenreRequest genreRequest)
        {
            var genre = dbContext.Genres.Where(r => r.Id == id).FirstOrDefault();
            if (genre == null)
                return null;

            genre.Name = genreRequest.Name;
            genre.Description = genreRequest.Description;

            dbContext.Update(genre);
            var isSuccess = Save();
            if (!isSuccess) return null;
            return new GenreDTO(genre);

        }

        public bool DeleteGenre(int id)
        {
            var genreToDelete = dbContext.Genres.FirstOrDefault(x => x.Id == id);
            if (genreToDelete == null)
                return false;

            dbContext.Remove(genreToDelete);
            return Save();
        }

        public GenreDTO GetGenreByName(string name)
        {
            var genre = dbContext.Genres.Where(g => g.Name == name).FirstOrDefault();
            return new GenreDTO(genre);
        }
    }
}
