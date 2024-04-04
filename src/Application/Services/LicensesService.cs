using Core.Repositories.Specific;
using Core.Services;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.DTOs.Licenses.Create;
using Models.DTOs.Licenses.GetAll;
using Models.DTOs.Licenses.GetById;
using Models.DTOs.Licenses.GetByIdLicenses;
using Models.DTOs.Licenses.Update;
using Models.DTOs.Tickets.GetAll;
using Models.Entities;
using Models.Enums;

namespace Business.Services
{
    public class LicensesService : ILicensesServices
	{
		private readonly ILicensesRepository _licensesRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public LicensesService(ILicensesRepository licensesRepository, IUserRepository userRepository, IProductRepository productRepository)
        {
            _licensesRepository = licensesRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<LicensesCreateResponseDto> CreateLicenses(int id, LicensesCreateDto request)
        {
            var NewLicenses = new Licenses
            {
                ProductId = request.ProductId,
                UserCount = request.UserCount,
                LicenseStatus=LiscenseStatus.Active,
                ExpireDate = DateTime.Now.AddYears(request.Year),
                UserId=id
            };
            await _licensesRepository.AddAsync(NewLicenses);
            var response = new LicensesCreateResponseDto
            {
                ProductId = request.ProductId,
                UserCount = NewLicenses.UserCount,
                ExpireDate = NewLicenses.ExpireDate
            };
            return response;
        }

        public LicensesUpdateStatusResponseDto Edit(LicensesUpdateStatusDto request)
        {
            var existingLicense = _licensesRepository.GetSingle(t => t.Id == request.Id);
            existingLicense.LicenseStatus = request.LicensesStatus;
            _licensesRepository.Edit(existingLicense);
            _licensesRepository.Save();
            return new LicensesUpdateStatusResponseDto
            {
                Id = existingLicense.Id,
                LicensesStatus = existingLicense.LicenseStatus,
            };
        }

        public List<LicensesGetAllResponseDto> GetAll()
        {
            var licensesWithUser = _licensesRepository.GetAll()
                .Join(_userRepository.GetAll(),
          l => l.UserId,
          user => user.Id,
          (l, user) => new LicensesGetAllResponseDto
          {
              ActivationDate = l.ActivationDate,
              UserCount = l.UserCount,
              ExpireDate = l.ExpireDate,
              licenseStatus = l.LicenseStatus.ToString(),
              UserEmail = user.Email
          })
         .ToList();
            return licensesWithUser;
        }
        public List<LicensesGetAllResponseDto> GetLicensesPagingData([FromQuery] PagedParameters prodParam)
        {
            var licensesWithUser = _licensesRepository.GetLicenses(prodParam)
                .Join(_userRepository.GetAll(),
          l => l.UserId,
          user => user.Id,
          (l, user) => new LicensesGetAllResponseDto
          {
              ActivationDate = l.ActivationDate,
              UserCount = l.UserCount,
              ExpireDate = l.ExpireDate,
              licenseStatus = l.LicenseStatus.ToString(),
              UserEmail = user.Email
          })
         .ToList();
            return licensesWithUser;
        }

        public List<GetLicensesResponseDto> GetById(int id)
       {
            var userLicenses = _licensesRepository.GetAll(x => x.UserId == id).ToList();
            var responseList = new List<GetLicensesResponseDto>();
            foreach (var item in userLicenses)
            {
                var productName = _productRepository.GetSingle(x => x.Id == item.ProductId)?.ProductName;
                if(item.ExpireDate < DateTime.Now)
                {
                   item.LicenseStatus=Models.Enums.LiscenseStatus.Expired;
                    _licensesRepository.Edit(item);
                    _licensesRepository.Save();
                }
                    var response = new GetLicensesResponseDto
                    {
                        Id = item.Id,
                        ProductName = productName,
                        ExpireDate = item.ExpireDate,
                        UserCount = item.UserCount,
                        licenseStatus = item.LicenseStatus.ToString()
                    };
                    responseList.Add(response);
            }
            return responseList;
        }

        public GetByIdLicensesResponseDto GetByIdLicenses(int id, int LicensesId)
        {
            var user = _userRepository.GetSingle(x=>x.Id == id);
           
                var license = _licensesRepository.GetSingle(x => x.Id == LicensesId);
                var ProductName = _productRepository.GetSingle(x => x.Id == license.ProductId).ProductName;

            var response = new GetByIdLicensesResponseDto
            {
                ProductName = ProductName,
                UsersCount = license.UserCount,
                ExpiryDate = license.ExpireDate,
                ActivationDate = license.ActivationDate,
                LiscenseStatus = license.LicenseStatus.ToString()
            };
                return response;
        }

       

        public List<string> GetLicensesStatus()
        {
            return Enum.GetNames(typeof(LiscenseStatus)).ToList();
        }
    }
}
