using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Users.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception("Không tìm thấy người dùng với ID này.");
            }

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
