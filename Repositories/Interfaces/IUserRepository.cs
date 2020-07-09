using cinema_core.DTOs.UserDTOs;
using cinema_core.Form;
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
        User GetUserById(int id);
        UserDTO CreateUser(UserRequest userRequest);
        UserDTO UpdateUser(int id,UserRequest userRequest);
        UserDTO UpdateRole(UpdateRoleRequest request);

        List<Role> GetAllRoles();
    }
}
