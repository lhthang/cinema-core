using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cinema_core.Models;
using cinema_core.Repositories;
using cinema_core.Utils;
using cinema_core.Utils.Constants;
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
        [Authorize()]
        public IActionResult Get(int id)
        {
            var username = Constants.GetUsername(Request);
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