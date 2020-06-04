using cinema_core.DTOs.ScreenTypeDTO;
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
        ScreenTypeDTO GetScreenTypeById(int Id);
    }
}
