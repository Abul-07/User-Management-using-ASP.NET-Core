﻿using System.ComponentModel.DataAnnotations;

namespace UserManagement.Core.Dtos.Auth
{
    public class RegisterDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        public string Address { get; set; }

    }
}
