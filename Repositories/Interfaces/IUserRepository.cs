using cinema_core.DTOs.UserDTOs;
using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories
{
    public interface IUserRepository
    {
        ICollection<Role> GetRolesOfUser(int id);
        ICollection<UserDTO> GetAllUsers();
        UserDTO GetUserByUsername(string username);
        User GetUser(string username);
        UserDTO GetUserById(int id);
    }
}
