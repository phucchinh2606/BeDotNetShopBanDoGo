using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.Users.CreateUser
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public UserRole Role { get; set; } // Admin có quyền chọn Role cho user mới
    }
}
