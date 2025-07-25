﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneDataAccess.DTOs
{
    public class UserCreateDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "User";

    }

    public class UserLoginDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
    }
}

