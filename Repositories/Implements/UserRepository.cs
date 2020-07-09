
using cinema_core.DTOs.UserDTOs;
using cinema_core.ErrorHandle;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Models.Base;
using cinema_core.Repositories.Base;
using cinema_core.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class UserRepository : BaseRepository, IUserRepository
    {

        public UserRepository(MyDbContext context) : base(context)
        {
        }
        public ICollection<Role> GetRolesOfUser(int id)
        {
            return dbContext.UserRole.Where(u => u.UserId == id).Select(r => r.Role).ToList();
        }

        public UserDTO GetUserByUsername(string username)
        {
            var user= dbContext.Users.Where(u => u.Username == username).FirstOrDefault();
            if (user == null) throw new CustomException(System.Net.HttpStatusCode.NotFound,"User not found");
            return new UserDTO(user);
        }

        public User GetUserById(int id)
        {
            var user= dbContext.Users.Where(u => u.Id == id).Include(ur => ur.UserRoles).ThenInclude(r => r.Role).FirstOrDefault();
            if (user == null) throw new CustomException(System.Net.HttpStatusCode.NotFound, "User not found");
            return user;
        }

        public ICollection<UserDTO> GetAllUsers()
        {
            List<UserDTO> userDTOs = new List<UserDTO>();
            var usersList = dbContext.Users.Include(ur => ur.UserRoles).ThenInclude(r => r.Role).OrderBy(u => u.Id).ToList();
            foreach (var user in usersList)
            {
                userDTOs.Add(new UserDTO(user));
            }
            return userDTOs;
        }

        public User GetUser(string username)
        {
            return dbContext.Users.Where(u => u.Username == username).FirstOrDefault();
        }

        public UserDTO UpdateRole(UpdateRoleRequest request)
        {
            var user = dbContext.Users.Where(u => u.Username == request.Username).FirstOrDefault();
            if (user == null) throw new CustomException(HttpStatusCode.NotFound, "user not found");

            var userRolesToDelete = dbContext.UserRole.Where(ur => ur.UserId == user.Id).ToList();
            if (userRolesToDelete != null)
                dbContext.RemoveRange(userRolesToDelete);

            var roles = dbContext.Roles.Where(r => request.RoleIds.Contains(r.Id)).ToList();

            foreach(var role in roles)
            {
                UserRole userRole = new UserRole()
                {
                    User = user,
                    Role = role,
                };
                dbContext.Add(userRole);
            }
            dbContext.SaveChanges();
            return new UserDTO(user);
        }

        public UserDTO CreateUser(UserRequest userRequest)
        {
            var user = new User()
            {
                Username = userRequest.Username,
                Email = userRequest.Email,
                Password = userRequest.Password,
                FullName = userRequest.FullName,
            };

            var roles = dbContext.Roles.Where(r => r.Name.ToLower() == "User".ToLower()).ToList();

            foreach(var role in roles)
            {
                UserRole userRole = new UserRole()
                {
                    Role = role,
                    User = user,
                };
                dbContext.Add(userRole);
            }
            dbContext.Add(user);
            Save();
            return new UserDTO(user);
        }

        public UserDTO UpdateUser(int id, UserRequest userRequest)
        {
            var user = dbContext.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null) throw new CustomException(HttpStatusCode.NotFound, "user not found");
            var username = user.Username;
            Coppier<UserRequest, User>.Copy(userRequest, user);
            user.Username = username;
            dbContext.Update(user);
            Save();
            return new UserDTO(user);
        }

        public List<Role> GetAllRoles()
        {
            return dbContext.Roles.OrderBy(r => r.Id).ToList();
        }
    }
}
