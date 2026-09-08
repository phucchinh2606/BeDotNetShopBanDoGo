using Application.Commons.DTOs;
using MediatR;

namespace Application.Queries.Users.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public Guid UserId { get; set; }

        public GetUserByIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
