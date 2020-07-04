using cinema_core.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace cinema_core.Utils.MovieProxy
{
    public static class MovieProxy
    {
        public static MovieResponse GetMovieByIMDB(string imdb)
        {
            string result = new WebClient().DownloadString("http://www.omdbapi.com/?apikey=b9d9362f&i=" + imdb);
            MovieResponse movie = new MovieResponse();
            dynamic json = JsonConvert.DeserializeObject(result);
            movie.Imdb = imdb;
            movie.Title = json["Title"];
            movie.Country = json["Country"];
            movie.Poster = json["Poster"];
            movie.RateName = json["Rated"];

            string list = json["Runtime"];
            if (!list.Contains("N/A"))
                movie.Runtime = int.Parse(list.Split(" min")[0]);
            else
                movie.Runtime = 0;

            list = json["Director"];
            List<string> directors = list.Split(", ").ToList();
            movie.Directors = directors.ToArray();

            list = json["Released"];
            movie.ReleasedAt = DateTime.Parse(list);

            list = json["Language"];
            List<string> languages = list.Split(", ").ToList();
            movie.Languages = languages.ToArray();

            list = json["Actors"];
            List<string> actors = list.Split(", ").ToList();
            movie.Actors = new List<Actor>();
            foreach(var name in actors)
            {
                Actor actor = new Actor()
                {
                    Name = name,
                    Avatar = "",
                };
                movie.Actors.Add(actor);
            }

            list = json["Genre"];
            List<string> genres = list.Split(", ").ToList();
            movie.Genres = new List<Genre>();
            foreach (var name in genres)
            {
                Genre genre = new Genre()
                {
                    Name = name,
                    Description = "",
                };
                movie.Genres.Add(genre);
            }

            return movie;
        }
    }
}
