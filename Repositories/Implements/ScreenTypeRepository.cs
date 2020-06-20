using cinema_core.DTOs.ScreenTypeDTOs;
using cinema_core.Form;
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

        private bool Save()
        {
            var save = dbContext.SaveChanges();
            return save > 0;
        }

        public ScreenTypeDTO CreateScreenType(ScreenTypeRequest screenTypeRequest)
        {
            var screenType = new ScreenType()
            {
                Name = screenTypeRequest.Name,
            };

            dbContext.Add(screenType);
            var isSuccess = Save();
            if (!isSuccess) return null;
            return new ScreenTypeDTO(screenType);
        }

        public ScreenTypeDTO UpdateScreenType(int id, ScreenTypeRequest screenTypeRequest)
        {
            var screenType = dbContext.ScreenTypes.Where(r => r.Id == id).FirstOrDefault();
            if (screenType == null)
                return null;

            screenType.Name = screenTypeRequest.Name;

            dbContext.Update(screenType);
            var isSuccess = Save();
            if (!isSuccess) return null;
            return new ScreenTypeDTO(screenType);
        }

        public bool DeleteScreenType(int id)
        {
            var screenTypeToDelete = dbContext.ScreenTypes.FirstOrDefault(x => x.Id == id);
            if (screenTypeToDelete == null)
                return false;

            dbContext.Remove(screenTypeToDelete);
            return Save();
        }
    }
}
