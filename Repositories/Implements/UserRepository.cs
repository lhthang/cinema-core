
using cinema_core.Models;
using cinema_core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class UserRepository : IUserRepository
    {
        private MyDbContext myDbContext;

        public UserRepository(MyDbContext context)
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

        public User GetUserById(int id)
        {
            return myDbContext.Users.Where(u => u.Id == id).FirstOrDefault();
        }
    }
}
