using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Payments;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Core.IServices;
using DoctorsClinic.Core.IServices.Account;
using DoctorsClinic.Domain.Entities;
using DoctorsClinic.Infrastructure.IRepositories;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DoctorsClinic.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly IUserAccessorService _userAccessor;
        public PaymentService(IRepositoryWrapper wrapper, IUserAccessorService userAccessor)
        {
            _wrapper = wrapper;
            _userAccessor = userAccessor;
        }

        public async Task<ResponseDto<PaginationDto<PaymentDto>>> GetAll(PaginationQuery paginationQuery, PaymentFilterDto filter)
        {
            #region Apply Filter
            var query = _wrapper.PaymentRepo.GetAll()
                .Include(pa => pa.Invoice)
                .Where(pa => !filter.InvoiceID.HasValue || pa.InvoiceID == filter.InvoiceID)
                .Where(pa => !filter.Amount.HasValue || pa.Amount == filter.Amount)
                .Where(pa => !filter.PaymentMethod.HasValue || pa.PaymentMethod == filter.PaymentMethod);
            #endregion

            var data = await query
                .OrderByDescending(pa => pa.CreatedAt)
                .ApplyPagging(paginationQuery)
                .ProjectToType<PaymentDto>()
                .ToListAsync();

            var count = await query.CountAsync();
            var metadata = new PaginationMetadata(count, paginationQuery);

            return new ResponseDto<PaginationDto<PaymentDto>>(
                new PaginationDto<PaymentDto>(data, metadata));
        }

        public async Task<ResponseDto<PaymentResponseDto>> GetById(int id)
        {
            var payment = await _wrapper.PaymentRepo.FindByCondition(pa => pa.Id == id)
                .Include(pa => pa.Invoice)
                .FirstOrDefaultAsync();

            if (payment == null)
                return new ResponseDto<PaymentResponseDto>(MsgResponce.Payment.NotFound, true);

            return new ResponseDto<PaymentResponseDto>(payment.Adapt<PaymentResponseDto>());
        }

        public async Task<ResponseDto<PaymentDto>> Add(CreatePaymentDto form)
        {
            var invoice = await _wrapper.InvoiceRepo.FindItemByCondition(inv => inv.Id == form.InvoiceID);
            if (invoice == null)
                return new ResponseDto<PaymentDto>(MsgResponce.Invoice.NotFound, true);

            if (form.Amount <= 0)
                return new ResponseDto<PaymentDto>(MsgResponce.AlarmAmounts.WrongEntry);

            var payment = form.Adapt<Payment>();
            payment.CreatorId = _userAccessor.UserId;
            payment.CreatedAt = DateTime.Now;

            await _wrapper.PaymentRepo.Insert(payment);
            await _wrapper.SaveAllAsync();

            payment = await _wrapper.PaymentRepo.FindByCondition(pa => pa.Id == payment.Id)
                .Include(pa => pa.Invoice)
                .FirstOrDefaultAsync();

            return new ResponseDto<PaymentDto>(payment.Adapt<PaymentDto>());
        }

        public async Task<ResponseDto<PaymentDto>> Update(int id, UpdatePaymentDto form)
        {
            var payment = await _wrapper.PaymentRepo.FindByCondition(pa => pa.Id == id)
                .Include(pa => pa.Invoice)
                .FirstOrDefaultAsync();
            if (payment == null)
                return new ResponseDto<PaymentDto>(MsgResponce.Payment.NotFound, true);

            var invoice = await _wrapper.InvoiceRepo.FindItemByCondition(inv => inv.Id == form.InvoiceID);
            if (invoice == null)
                return new ResponseDto<PaymentDto>(MsgResponce.Invoice.NotFound, true);

            if (form.Amount <= 0)
                return new ResponseDto<PaymentDto>(MsgResponce.AlarmAmounts.WrongEntry);

            var savePayment = form.Adapt(payment);
            savePayment.ModifierId = _userAccessor.UserId;
            savePayment.ModifieAt = DateTime.Now;

            await _wrapper.PaymentRepo.Update(savePayment);
            return new ResponseDto<PaymentDto>(savePayment.Adapt<PaymentDto>());
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            var payment = await _wrapper.PaymentRepo.FindItemByCondition(pa => pa.Id == id);
            if (payment == null)
                return new ResponseDto<bool>(MsgResponce.Payment.NotFound, true);

            payment.DeleterId = _userAccessor.UserId;
            await _wrapper.PaymentRepo.Delete(id);
            await _wrapper.SaveAllAsync();
            return new ResponseDto<bool>(true);
        }
    }
}
