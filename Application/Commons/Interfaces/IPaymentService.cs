using Application.Commons.Models;
using Domain.Entities;
using Domain.Enums;

namespace Application.Commons.Interfaces
{
    public interface IPaymentService
    {
        PaymentMethod Method { get; }
        Task<PaymentResult> ProcessPaymentAsync(Order order);
    }
}
