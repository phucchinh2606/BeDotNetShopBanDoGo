using Application.Commons.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Users.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var dto = request.PasswordDto;

            if (dto.NewPassword != dto.ConfirmNewPassword)
            {
                throw new BadRequestException("Mật khẩu mới và mật khẩu xác nhận không trùng khớp.");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new NotFoundException("Người dùng", request.UserId);
            }

            // Kiểm tra mật khẩu hiện tại
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new BadRequestException("Mật khẩu hiện tại không chính xác.");
            }

            // Hash mật khẩu mới
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
