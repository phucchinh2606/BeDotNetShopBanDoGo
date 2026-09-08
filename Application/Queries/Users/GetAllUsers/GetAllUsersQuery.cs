using Application.Commons.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Queries.Users.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<List<UserDto>>
    {
    }
}
