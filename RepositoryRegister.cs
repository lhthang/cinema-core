using cinema_core.Repositories;
using cinema_core.Repositories.Implements;
using cinema_core.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core
{
    public static class RepositoryRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IScreenTypeRepository, ScreenTypeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IClusterRepository, ClusterRepository>();
            services.AddScoped<IRateRepository, RateRepository>();
            services.AddScoped<IShowtimeRepository, ShowtimeRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
        }
    }
}
