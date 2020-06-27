using cinema_core.DTOs.ScreenTypeDTOs;
using cinema_core.ErrorHandle;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Models.Base;
using cinema_core.Repositories.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class ScreenTypeRepository : BaseRepository, IScreenTypeRepository
    {
        public ScreenTypeRepository(MyDbContext context) : base(context) { }

        public ScreenTypeDTO GetScreenTypeById(int id)
        {
            var screenType = GetScreenTypeEntityById(id);
            return new ScreenTypeDTO(screenType);
        }

        public ICollection<ScreenTypeDTO> GetScreenTypes()
        {
            List<ScreenTypeDTO> results = new List<ScreenTypeDTO>();
            var screenTypes = dbContext.ScreenTypes.OrderBy(sc => sc.Id).ToList();
            foreach (ScreenType screenType in screenTypes)
            {
                results.Add(new ScreenTypeDTO(screenType));
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

        public ScreenTypeDTO CreateScreenType(ScreenTypeRequest screenTypeRequest)
        {
            CheckNameDupplicate(screenTypeRequest.Name);

            var screenType = new ScreenType()
            {
                Name = screenTypeRequest.Name,
            };

            dbContext.Add(screenType);
            Save();
            return new ScreenTypeDTO(screenType);
        }

        public ScreenTypeDTO UpdateScreenType(int id, ScreenTypeRequest screenTypeRequest)
        {
            var screenType = GetScreenTypeEntityById(id);
            CheckNameDupplicate(screenTypeRequest.Name);

            screenType.Name = screenTypeRequest.Name;

            dbContext.Update(screenType);
            Save();
            return new ScreenTypeDTO(screenType);
        }

        public bool DeleteScreenType(int id)
        {
            var screenTypeToDelete = GetScreenTypeEntityById(id);

            dbContext.Remove(screenTypeToDelete);
            Save();
            return true;
        }

        private ScreenType GetScreenTypeEntityById(int id)
        {
            var screenType = dbContext.ScreenTypes.Where(r => r.Id == id).FirstOrDefault();
            if (screenType == null)
                throw new CustomException(HttpStatusCode.NotFound,"Id not found.");

            return screenType;
        }

        private void CheckNameDupplicate(string name)
        {
            var screenTypeByName = dbContext.ScreenTypes.Where(sc => sc.Name == name).FirstOrDefault();
            if (screenTypeByName != null)
                throw new CustomException(HttpStatusCode.BadRequest,$"Screen Type {name} already existed.");
        }
    }
}
