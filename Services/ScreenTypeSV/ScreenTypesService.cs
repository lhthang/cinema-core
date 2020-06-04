using cinema_core.DTOs.ScreenTypeDTO;
using cinema_core.Models;
using cinema_core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Services.ScreenTypeSV
{
    public class ScreenTypesService : IScreenTypeRepository
    {
        private MyDbContext dbContext;

        public ScreenTypesService(MyDbContext context)
        {
            dbContext = context;
        }
        public ScreenTypeDTO GetScreenTypeById(int Id)
        {
            var screenType = dbContext.ScreenTypes.Where(sc => sc.Id == Id).FirstOrDefault();
            if (screenType == null)
                return null;
            return new ScreenTypeDTO()
            {
                Id = screenType.Id,
                Name = screenType.Name,
            };
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
    }
}
