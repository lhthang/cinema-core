using cinema_core.DTOs.ScreenTypeDTOs;
using cinema_core.Models;
using cinema_core.Models.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class ScreenTypeRepository : IScreenTypeRepository
    {
        private MyDbContext dbContext;

        public ScreenTypeRepository(MyDbContext context)
        {
            dbContext = context;
        }

        public bool CreateScreenType(ScreenType screenType)
        {
            dbContext.AddAsync(screenType);
            return Save();
        }

        public bool DeleteScreenType(ScreenType screenType)
        {
            dbContext.Remove(screenType);
            return Save();
        }

        public ScreenType GetScreenTypeById(int Id)
        {
            var screenType = dbContext.ScreenTypes.Where(sc => sc.Id == Id).FirstOrDefault();
            return screenType;
        }

        public ScreenType GetScreenTypeByName(string name)
        {
            var screenType = dbContext.ScreenTypes.Where(sc => sc.Name == name).FirstOrDefault();
            return screenType;
        }

        public ICollection<ScreenTypeDTO> GetScreenTypes()
        {
            List<ScreenTypeDTO> results = new List<ScreenTypeDTO>();
            var screenTypes = dbContext.ScreenTypes.OrderBy(sc => sc.Id).ToList();
            foreach (ScreenType screenType in screenTypes)
            {
                results.Add(new ScreenTypeDTO()
                {
                    Id = screenType.Id,
                    Name = screenType.Name,
                });
            }
            return results;
        }

        public ICollection<ScreenTypeDTO> GetScreenTypesByMovieId(int movieId)
        {
            List<ScreenTypeDTO> results = new List<ScreenTypeDTO>();
            var movieScreenTypes = dbContext.MovieScreenTypes
                                    .Where(mst => mst.MovieId == movieId)
                                    .Include(mst => mst.ScreenType)
                                    .OrderBy(mst => mst.ScreenTypeId)
                                    .ToList();
            foreach (MovieScreenType movieScreenType in movieScreenTypes)
            {
                results.Add(new ScreenTypeDTO()
                {
                    Id = movieScreenType.ScreenTypeId,
                    Name = movieScreenType.ScreenType.Name
                }); ;
            }
            return results;
        }

        public ICollection<ScreenTypeDTO> GetScreenTypesByRoomId(int roomId)
        {
            List<ScreenTypeDTO> results = new List<ScreenTypeDTO>();
            var roomScreenTypes = dbContext.RoomScreenTypes
                                    .Where(rst => rst.RoomId == roomId)
                                    .Include(rst => rst.ScreenType)
                                    .OrderBy(rst => rst.ScreenTypeId)
                                    .ToList();
            foreach (RoomScreenType roomScreenType in roomScreenTypes)
            {
                results.Add(new ScreenTypeDTO()
                {
                    Id = roomScreenType.ScreenTypeId,
                    Name = roomScreenType.ScreenType.Name
                }); ;
            }
            return results;
        }

        public bool Save()
        {
            var save = dbContext.SaveChanges();
            return save > 0;
        }

        public bool UpdateScreenType(ScreenType screenType)
        {
            dbContext.Update(screenType);
            return Save();
        }
    }
}
