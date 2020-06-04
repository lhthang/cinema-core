using cinema_core.Models.User;
using cinema_core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Services.UserSV
{
    public class UsersService : IUserRepository
    {
        private MyDbContext myDbContext;

        public UsersService(MyDbContext context)
        {
            myDbContext = context;
        }

        public ICollection<Role> GetRolesOfUser(int id)
        {
            return myDbContext.UserRole.Where(u => u.UserId == id).Select(r => r.Role).ToList();
        }

        public User GetUserByUsername(string username)
        {
            return myDbContext.Users.Where(u => u.Username == username).FirstOrDefault();
        }
    }
}
