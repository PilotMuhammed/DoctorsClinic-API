using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DoctorsClinic.Core.Dtos.Medicines;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IMedicineService : IScopedService
    {
        Task<ResponseDto<PaginationDto<MedicineDto>>> GetAllAsync(
            PaginationQuery pagination,
            MedicineFilterDto filter,
            CancellationToken ct = default);
        Task<ResponseDto<IEnumerable<ListDto<int>>>> GetListAsync(CancellationToken ct = default);
        Task<ResponseDto<MedicineResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ResponseDto<MedicineDto>> CreateAsync(CreateMedicineDto dto, CancellationToken ct = default);
        Task<ResponseDto<MedicineDto>> UpdateAsync(int id, UpdateMedicineDto dto, CancellationToken ct = default);
        Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default);
    }
}
