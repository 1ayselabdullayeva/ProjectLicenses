using Models.DTOs.Licenses.Create;
using Models.DTOs.Licenses.GetById;
using Models.DTOs.Licenses.GetByIdLicenses;
using Models.DTOs.Licenses.Update;
using Models.DTOs.Tickets.Create;
using Models.DTOs.Tickets.Edit;
using Models.DTOs.Tickets.GetById;

namespace Core.Services
{
    public interface ILicensesServices
	{
        public List<GetLicensesResponseDto> GetById(int id);
        public Task<LicensesCreateResponseDto> CreateLicenses(int id, LicensesCreateDto request);
        public GetByIdLicensesResponseDto GetByIdLicenses(int id,int LicensesId);
        List<string> GetLicensesStatus();
        LicensesUpdateStatusResponseDto Edit(LicensesUpdateStatusDto request);
    }
}
