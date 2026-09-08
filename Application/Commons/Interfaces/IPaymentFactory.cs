using Domain.Enums;

namespace Application.Commons.Interfaces
{
    public interface IPaymentFactory
    {
        IPaymentService GetPaymentService(PaymentMethod method);
    }
}
