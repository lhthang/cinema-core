using cinema_core.DTOs.ScreenTypeDTOs;
using cinema_core.Form;
using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories
{
    public interface IScreenTypeRepository
    {
        ICollection<ScreenTypeDTO> GetScreenTypes();
        ICollection<ScreenTypeDTO> GetScreenTypesByMovieId(int movieId);
        ICollection<ScreenTypeDTO> GetScreenTypesByRoomId(int roomId);
        ScreenTypeDTO GetScreenTypeById(int Id);
        ScreenTypeDTO CreateScreenType(ScreenTypeRequest screenTypeRequest);
        ScreenTypeDTO UpdateScreenType(int id, ScreenTypeRequest screenTypeRequest);
        bool DeleteScreenType(int id);
    }
}
