using DoctorsClinic.Core.Dtos.Medicines;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IMedicineService : IScopedService
    {
        Task<ResponseDto<PaginationDto<MedicineDto>>> GetAll(PaginationQuery paginationQuery, MedicineFilterDto filter);
        Task<ResponseDto<IEnumerable<ListDto<int>>>> GetList();
        Task<ResponseDto<MedicineResponseDto>> GetById(int id);
        Task<ResponseDto<MedicineDto>> Add(CreateMedicineDto form);
        Task<ResponseDto<MedicineDto>> Update(int id, UpdateMedicineDto form);
        Task<ResponseDto<bool>> Delete(int id);
    }
}
