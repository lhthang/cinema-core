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
        public static void GetMovieByIMDB(string imdb)
        {
            string result = new WebClient().DownloadString("http://www.omdbapi.com/?apikey=b9d9362f&i=" + "tt4154796");
            MovieResponse movie = new MovieResponse();
            dynamic json = JsonConvert.DeserializeObject(result);
            movie.Title = json["Title"];

            string list = json["Actors"];
            List<string> actors = list.Split(", ").ToList();
            movie.Actors = actors;
        }
    }
}
