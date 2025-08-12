using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DoctorsClinic.Core.Dtos.Specialties;
using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface ISpecialtyService : IScopedService
    {
        Task<ResponseDto<PaginationDto<SpecialtyDto>>> GetAllAsync(
            PaginationQuery pagination,
            SpecialtyFilterDto filter,
            CancellationToken ct = default);
        Task<ResponseDto<IEnumerable<ListDto<int>>>> GetListAsync(CancellationToken ct = default);
        Task<ResponseDto<SpecialtyResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ResponseDto<SpecialtyDto>> CreateAsync(CreateSpecialtyDto dto, CancellationToken ct = default);
        Task<ResponseDto<SpecialtyDto>> UpdateAsync(int id, UpdateSpecialtyDto dto, CancellationToken ct = default);
        Task<ResponseDto<bool>> DeleteAsync(int id, CancellationToken ct = default);
    }
}
