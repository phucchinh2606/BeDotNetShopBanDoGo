using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.Auth.Login
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
