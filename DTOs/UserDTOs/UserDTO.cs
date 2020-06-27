using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.DTOs.UserDTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<RoleDTO> Roles { get; set; }
        public UserDTO() { }
        public UserDTO(User user)
        {
            Id = user.Id;
            FullName = user.FullName;
            Username = user.Username;
            Email = user.Email;

            List<RoleDTO> roleDTOs = new List<RoleDTO>();
            if (user.UserRoles != null)
            {
                foreach (var userRole in user.UserRoles)
                {
                    RoleDTO role = new RoleDTO(userRole.Role);
                    roleDTOs.Add(role);
                }
            }
            Roles = roleDTOs;
        }
    }
}
