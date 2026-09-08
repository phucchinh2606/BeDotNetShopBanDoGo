using Domain.Enums;
using MediatR;

namespace Application.Commands.Users.UpdateUser
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public UserRole Role { get; set; }
    }
}
