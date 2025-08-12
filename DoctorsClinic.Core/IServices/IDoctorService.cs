using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DoctorsClinic.Core.Dtos.Doctors;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface IDoctorService : IScopedService
    {
        Task<ResponseDto<PaginationDto<DoctorDto>>> GetAllAsync(
            PaginationQuery pagination,
            DoctorFilterDto filter,
            CancellationToken ct = default);
        Task<ResponseDto<IEnumerable<ListDto<int>>>> GetListAsync(CancellationToken ct = default);
        Task<ResponseDto<List<DoctorDto>>> GetBySpecialtyAsync(int specialtyId, CancellationToken ct = default);
        Task<ResponseDto<DoctorResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ResponseDto<DoctorDto>> CreateAsync(CreateDoctorDto dto, CancellationToken ct = default);
        Task<ResponseDto<DoctorDto>> UpdateAsync(int id, UpdateDoctorDto dto, CancellationToken ct = default);
        Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default);
    }
}
