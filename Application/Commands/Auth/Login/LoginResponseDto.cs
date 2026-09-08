using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.Auth.Login
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
