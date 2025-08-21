using DoctorsClinic.Core.Dtos.Invoices;
using DoctorsClinic.Core.Dtos.Payments;
using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Domain.Enums; 
using Mapster;

namespace DoctorsClinic.Core.Mapster
{
    public class PaymentMap
    {
        public static void Configure()
        {

            TypeAdapterConfig<Payment, PaymentDto>.NewConfig()
                .Map(d => d.PaymentMethod, s => s.PaymentMethod.ToString());

            TypeAdapterConfig<PaymentDto, Payment>.NewConfig()
                .Map(d => d.PaymentMethod, s => ParsePaymentMethod(s.PaymentMethod))
                .Ignore(d => d.Invoice!);

            TypeAdapterConfig<CreatePaymentDto, Payment>.NewConfig()
                .Map(d => d.PaymentMethod, s => ParsePaymentMethod(s.PaymentMethod))
                .Ignore(d => d.PaymentID)
                .Ignore(d => d.Invoice!);

            TypeAdapterConfig<UpdatePaymentDto, Payment>.NewConfig()
                .IgnoreIf((s, _) => s.InvoiceID == null, d => d.InvoiceID)
                .IgnoreIf((s, _) => s.Amount == null, d => d.Amount)
                .IgnoreIf((s, _) => s.Date == null, d => d.Date)
                .IgnoreIf((s, _) => string.IsNullOrWhiteSpace(s.PaymentMethod), d => d.PaymentMethod)
                .Map(d => d.PaymentMethod, s => string.IsNullOrEmpty(s.PaymentMethod) ? default : ParsePaymentMethod(s.PaymentMethod))
                .Ignore(d => d.Invoice!);

            TypeAdapterConfig<Payment, PaymentResponseDto>.NewConfig()
                .Map(d => d.Payment, s => s.Adapt<PaymentDto>()!) 
                .Map(d => d.Invoice, _ => (InvoiceDto?)null);
        }

        private static PaymentMethod ParsePaymentMethod(string? method)
        {
            return Enum.TryParse<PaymentMethod>(method, true, out var value) ? value : default;
        }
    }
}
