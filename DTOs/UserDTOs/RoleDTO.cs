using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.DTOs.UserDTOs
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public RoleDTO() { }
        public RoleDTO(Role role)
        {
            Id = role.Id;
            Role = role.Name;
        }
    }
}
