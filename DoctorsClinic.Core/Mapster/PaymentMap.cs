using DoctorsClinic.Core.Dtos.Invoices;
using DoctorsClinic.Core.Dtos.Payments;
using DoctorsClinic.Domain.Entities;
using Mapster;

namespace DoctorsClinic.Core.Mapster
{
    public class PaymentMap
    {
        public static void Configure()
        {
            TypeAdapterConfig<CreatePaymentDto, Payment>.NewConfig();

            TypeAdapterConfig<Payment, PaymentResponseDto>.NewConfig()
                .Map(dest => dest.Payment, src => src.Adapt<PaymentDto>()!) 
                .Map(dest => dest.Invoice, src => src.Invoice.Adapt<InvoiceDto>());

            TypeAdapterConfig<Payment, PaymentDto>.NewConfig();

            TypeAdapterConfig<UpdatePaymentDto, Payment>.NewConfig()
                .IgnoreIf((src, dest) => src.InvoiceID == null, dest => dest.InvoiceID)
                .IgnoreIf((src, dest) => src.Amount == null, dest => dest.Amount)
                .IgnoreIf((src, dest) => src.PaymentMethod == null, dest => dest.PaymentMethod);
        }
    }
}
