using Application.Commons.Interfaces;
using Application.Commons.Models;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using PayOS;
using PayOS.Models.V2.PaymentRequests;

namespace Infrastructure.Services.Payments
{
    public class PayOsPaymentService : IPaymentService
    {
        private readonly PayOSClient _payOSClient;
        private readonly IConfiguration _configuration;

        public PayOsPaymentService(PayOSClient payOSClient, IConfiguration configuration)
        {
            _payOSClient = payOSClient;
            _configuration = configuration;
        }

        public PaymentMethod Method => PaymentMethod.PayOS;

        public async Task<PaymentResult> ProcessPaymentAsync(Order order)
        {
            try
            {
                // 1. Tạo orderCode dạng số long cho PayOS
                long orderCode = long.Parse(DateTime.UtcNow.ToString("yyMMddHHmmss"));

                // 2. Gán trực tiếp vào Order truyền vào để lưu vào DB
                order.OrderCode = orderCode;

                int amount = (int)Math.Round(order.TotalAmount);
                string baseUrl = _configuration["ClientUrl"] ?? "http://localhost:3000";

                var paymentRequest = new CreatePaymentLinkRequest
                {
                    OrderCode = orderCode,
                    Amount = amount,
                    Description = $"DH{orderCode}".Substring(0, Math.Min(25, $"DH{orderCode}".Length)),
                    CancelUrl = $"{baseUrl}/cart",
                    ReturnUrl = $"{baseUrl}/orders/{order.OrderId}?success=true"
                };

                // Gọi SDK PayOS
                var paymentLink = await _payOSClient.PaymentRequests.CreateAsync(paymentRequest);

                return new PaymentResult
                {
                    IsSuccess = true,
                    Message = "Tạo liên kết thanh toán payOS thành công.",
                    TransactionId = orderCode.ToString(),
                    PaymentUrl = paymentLink.CheckoutUrl
                };
            }
            catch (Exception ex)
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    Message = $"Lỗi tạo cổng thanh toán payOS: {ex.Message}"
                };
            }
        }
    }
}
