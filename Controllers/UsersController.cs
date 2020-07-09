using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using cinema_core.DTOs.UserDTOs;
using cinema_core.ErrorHandle;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Repositories;
using cinema_core.Utils;
using cinema_core.Utils.Constants;
using cinema_core.Utils.Password;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace cinema_core.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : Controller
    {
        private IUserRepository userRepository;
        private PasswordHasher passwordHasher;
        public UsersController(IUserRepository repository)
        {
            userRepository = repository;
            passwordHasher = new PasswordHasher();
        }

        // GET: api/users
        [HttpGet("[action]")]
        public IActionResult GetAllRoles()
        {
            try
            {
                var roles = userRepository.GetAllRoles();
                return Ok(roles);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // GET: api/users
        [HttpGet]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Get()
        {
            try
            {
                var users = userRepository.GetAllUsers();
                return Ok(users);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        [Authorize()]
        public IActionResult Get(int id)
        {
            var username = Constants.GetUsername(Request);
            try
            {
                var user = userRepository.GetUserById(id);
                return Ok(new UserDTO(user));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // GET: api/users/5
        [HttpPut("[action]")]
        [Authorize(Roles =Authorize.Admin)]
        public IActionResult UpdateRole([FromBody] UpdateRoleRequest updateRequest)
        {

            try
            {
                var user = userRepository.UpdateRole(updateRequest);
                return Ok(user);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPut("{id}")]
        [Authorize()]
        public IActionResult UpdateInformation(int id,[FromBody] UserRequest userRequest)
        {

            try
            {
                string username = Constants.GetUsername(Request);
                var isExist = userRepository.GetUserById(id);
                if (isExist.Username != username) throw new CustomException(HttpStatusCode.BadRequest, "can not update on different user");
                if (!passwordHasher.PasswordMatches(userRequest.Password, isExist.Password))
                {
                    var pwd = userRequest.Password;
                    userRequest.Password = passwordHasher.HashPassword(pwd);
                }
                else
                {
                    userRequest.Password = isExist.Password;
                }
                var user = userRepository.UpdateUser(id, userRequest);
                return Ok(user);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost("register")]
        public IActionResult SignUp([FromBody] UserRequest user)
        {
            try
            {
                var pwd = user.Password;
                user.Password = passwordHasher.HashPassword(pwd);
                var newUser = userRepository.CreateUser(user);
                return Ok(newUser);
            }catch(Exception e)
            {
                throw e;
            }
        }
    }
}