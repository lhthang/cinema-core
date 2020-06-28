using cinema_core.DTOs.GenreDTOs;
using cinema_core.DTOs.RateDTOs;
using cinema_core.DTOs.ScreenTypeDTOs;
using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.DTOs.MovieDTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
        public string Country { get; set; }
        public string[] Wallpapers { get; set; }
        public List<ActorDTO> Actors { get; set; }
        public string[] Directors { get; set; }

        public string[] Languages { get; set; }
        public string Story { get; set; }
        public string Imdb { get; set; }
        public int Runtime { get; set; }
        public DateTime ReleasedAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Trailer { get; set; }
        public List<ScreenTypeDTO> ScreenTypes { get; set; }
        public RateDTO Rate { get; set; }

        public List<GenreDTO> Genres { get; set; }

        public MovieDTO(Movie movie)
        {
            if (movie == null)
                return;

            this.Id = movie.Id;
            this.Country = movie.Country;
            this.Title = movie.Title;
            this.Runtime = movie.Runtime;
            this.Poster = movie.Poster;
            this.Trailer = movie.Trailer;
            this.Wallpapers = movie.Wallpapers;
            this.ReleasedAt = movie.ReleasedAt;
            this.Languages = movie.Languages;
            this.Story = movie.Story;
            this.EndAt = movie.EndAt;
            this.Directors = movie.Directors;
            this.Imdb = movie.Imdb;
            List<ScreenTypeDTO> list = new List<ScreenTypeDTO>();
            if (movie.MovieScreenTypes != null)
            {
                foreach (var movieScreenType in movie.MovieScreenTypes)
                {
                    var screenTypeDTO = new ScreenTypeDTO()
                    {
                        Id = movieScreenType.ScreenType.Id,
                        Name = movieScreenType.ScreenType.Name,
                    };
                    list.Add(screenTypeDTO);
                }
            }
            this.ScreenTypes = list;

            List<ActorDTO> actors = new List<ActorDTO>();
            if (movie.MovieActors != null)
            {
                foreach (var movieActors in movie.MovieActors)
                {
                    var actor = new ActorDTO()
                    {
                        Id = movieActors.Actor.Id,
                        Name = movieActors.Actor.Name,
                        Avatar = movieActors.Actor.Avatar,
                    };
                    actors.Add(actor);
                }
            }
            this.Actors = actors;

            List<GenreDTO> genreDTOs = new List<GenreDTO>();
            if (movie.MovieGenres != null)
            {
                foreach (var movieGenre in movie.MovieGenres)
                {
                    var genre = new GenreDTO(movieGenre.Genre);
                    genreDTOs.Add(genre);
                }
            }
            this.Genres = genreDTOs;

            if (movie.Rate!=null)
                this.Rate = new RateDTO(movie.Rate);
        }
    }
}
