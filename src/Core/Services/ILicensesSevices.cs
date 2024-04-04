using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.DTOs.Licenses.Create;
using Models.DTOs.Licenses.GetAll;
using Models.DTOs.Licenses.GetById;
using Models.DTOs.Licenses.GetByIdLicenses;
using Models.DTOs.Licenses.Update;
using Models.DTOs.Tickets.GetAll;

namespace Core.Services
{
    public interface ILicensesServices
	{
        List<GetLicensesResponseDto> GetById(int id);
        Task<LicensesCreateResponseDto> CreateLicenses(int id, LicensesCreateDto request);
        GetByIdLicensesResponseDto GetByIdLicenses(int id,int LicensesId);
        List<string> GetLicensesStatus();
        LicensesUpdateStatusResponseDto Edit(LicensesUpdateStatusDto request);
        List<LicensesGetAllResponseDto> GetAll();
        List<LicensesGetAllResponseDto> GetLicensesPagingData([FromQuery] PagedParameters prodParam);
    }
}
