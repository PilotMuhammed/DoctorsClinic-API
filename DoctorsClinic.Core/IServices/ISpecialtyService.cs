using DoctorsClinic.Core.Dtos;
using DoctorsClinic.Core.Dtos.Specialties;
using DoctorsClinic.Core.Helper;
using DoctorsClinic.Domain.Interfaces;

namespace DoctorsClinic.Core.IServices
{
    public interface ISpecialtyService : IScopedService
    {
        Task<ResponseDto<PaginationDto<SpecialtyDto>>> GetAll(PaginationQuery paginationQuery, SpecialtyFilterDto filter);
        Task<ResponseDto<IEnumerable<ListDto<int>>>> GetList();
        Task<ResponseDto<SpecialtyResponseDto>> GetById(int id);
        Task<ResponseDto<SpecialtyDto>> Add(CreateSpecialtyDto form);
        Task<ResponseDto<SpecialtyDto>> Update(int id, UpdateSpecialtyDto form);
        Task<ResponseDto<bool>> Delete(int id);
    }
}
