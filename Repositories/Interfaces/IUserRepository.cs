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
        User GetUserByUsername(string username);
        User GetUserById(int id);
    }
}
