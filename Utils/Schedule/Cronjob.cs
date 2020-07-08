using cinema_core.Repositories;
using cinema_core.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cinema_core.Utils.Schedule
{
    public class CronJob : BackgroundService
    {
        private readonly IServiceProvider _service;
        private CrontabSchedule _schedule;
        private DateTime _nextRun;


        //private string Schedule => "*/10 * * * * *"; //Runs every 10 seconds
        private string Schedule => "59 0/10 * * * *"; //Run every 10 minutes from 00:00

        public CronJob(IServiceProvider service)
        {
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
            _service = service;
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;
                var nextrun = _schedule.GetNextOccurrence(now);
                if (now > _nextRun)
                {
                    Process();
                    _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(5000, stoppingToken); //5 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        private void Process()
        {
            var promotionRepository = _service.CreateScope().ServiceProvider.GetRequiredService<IPromotionRepository>();
            var showtimeRepository = _service.CreateScope().ServiceProvider.GetRequiredService<IShowtimeRepository>();
            System.Diagnostics.Debug.WriteLine("Update at... "+DateTime.Now.ToString("F"));
            showtimeRepository.AutoUpdateShowtime();
            promotionRepository.AutoUpdatePromotion();
        }
    }
}
