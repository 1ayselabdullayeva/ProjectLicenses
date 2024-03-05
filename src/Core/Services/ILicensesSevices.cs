using Models.DTOs.Licenses.Create;
using Models.DTOs.Licenses.GetById;
using Models.DTOs.Tickets.Create;
using Models.DTOs.Tickets.GetById;

namespace Core.Services
{
    public interface ILicensesServices
	{
        //public GetLicensesResponseDto GetById(int id);
        public Task<LicensesCreateResponseDto> CreateLicenses(int id, LicensesCreateDto request);
    }
}
