using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cinema_core.Models;
using cinema_core.Repositories;
using cinema_core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cinema_core.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : Controller
    {
        private IUserRepository userRepository;
        public UsersController(IUserRepository repository)
        {
            userRepository = repository;
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
        [Authorize(Roles ="SSS")]
        public IActionResult Get(int id)
        {
            try
            {
                var user = userRepository.GetUserById(id);
                return Ok(user);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}