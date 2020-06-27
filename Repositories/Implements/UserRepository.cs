
using cinema_core.DTOs.UserDTOs;
using cinema_core.ErrorHandle;
using cinema_core.Models;
using cinema_core.Models.Base;
using Microsoft.EntityFrameworkCore;
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

        public UserDTO GetUserByUsername(string username)
        {
            var user= myDbContext.Users.Where(u => u.Username == username).FirstOrDefault();
            if (user == null) throw new CustomException(System.Net.HttpStatusCode.NotFound,"User not found");
            return new UserDTO(user);
        }

        public UserDTO GetUserById(int id)
        {
            var user= myDbContext.Users.Where(u => u.Id == id).Include(ur => ur.UserRoles).ThenInclude(r => r.Role).FirstOrDefault();
            if (user == null) throw new CustomException(System.Net.HttpStatusCode.NotFound, "User not found");
            return new UserDTO(user);
        }

        public ICollection<UserDTO> GetAllUsers()
        {
            List<UserDTO> userDTOs = new List<UserDTO>();
            var usersList = myDbContext.Users.Include(ur => ur.UserRoles).ThenInclude(r => r.Role).OrderBy(u => u.Id).ToList();
            foreach (var user in usersList)
            {
                userDTOs.Add(new UserDTO(user));
            }
            return userDTOs;
        }

        public User GetUser(string username)
        {
            return myDbContext.Users.Where(u => u.Username == username).FirstOrDefault();
        }
    }
}
