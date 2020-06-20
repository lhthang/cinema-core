using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.DTOs.ScreenTypeDTOs
{
    public class ScreenTypeDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ScreenTypeDTO() { }
        public ScreenTypeDTO(ScreenType screenType)
        {
            if (screenType == null)
                return;

            this.Id = screenType.Id;
            this.Name = screenType.Name;
        }
    }
}
