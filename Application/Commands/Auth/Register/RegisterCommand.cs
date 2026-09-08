using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.Auth.Register
{
    public class RegisterCommand : IRequest<bool>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
