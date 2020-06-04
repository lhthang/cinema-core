﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Form.User
{
    public class UserRequest
    {
        [Required]
        public string Username
        {
            get; set;
        }

        [Required]
        public string Password
        {
            get; set;
        }
    }
}
