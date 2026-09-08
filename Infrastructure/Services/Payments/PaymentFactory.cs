using Application.Commons.Interfaces;
using Domain.Enums;

namespace Infrastructure.Services.Payments
{
    public class PaymentFactory : IPaymentFactory
    {
        private readonly IEnumerable<IPaymentService> _paymentServices;

        public PaymentFactory(IEnumerable<IPaymentService> paymentServices)
        {
            _paymentServices = paymentServices;
        }

        public IPaymentService GetPaymentService(PaymentMethod method)
        {
            var service = _paymentServices.FirstOrDefault(s => s.Method == method);
            if (service == null)
            {
                throw new NotSupportedException($"Phương thức thanh toán '{method}' chưa được hỗ trợ.");
            }
            return service;
        }
    }
}
