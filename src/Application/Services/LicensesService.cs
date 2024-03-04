using Core.Repositories.Specific;
using Core.Services;
using DataAccessLayer.Repositories;
using Models.DTOs.Licenses.Create;
using Models.DTOs.Licenses.GetById;
using Models.Entities;

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
            var ProductId = _productRepository.GetSingle(x => x.ProductName == request.ProductName).Id;
            var NewLicenses = new Licenses
            {
                ProductId = ProductId,
                UserCount = request.UserCount,
                ExpireDate = DateTime.Now.AddYears(request.Year)
            };
            await _licensesRepository.AddAsync(NewLicenses);
            var users = _userRepository.GetSingle(x => x.Id == id);
            users.LicensesId=NewLicenses.Id;
            _userRepository.Edit(users);
            _userRepository.Save();
            var response = new LicensesCreateResponseDto
            {
                ProductId = ProductId,
                UserCount = NewLicenses.UserCount,
                ExpireDate = NewLicenses.ExpireDate
            };
            return response;
        }

        public GetLicensesResponseDto GetById(int id)
        {
            var LicensesId = _userRepository.GetSingle(x=>x.Id==id).LicensesId;
            var ProductId = _licensesRepository.GetSingle(x => x.Id == LicensesId).ProductId;
            var ProductName = _productRepository.GetSingle(x => x.Id == ProductId).ProductName;
            var Licenses = _licensesRepository.GetSingle(x=>x.Id== LicensesId);
            var LicenseStatus = _licensesRepository.GetSingle(x=>x.Id== LicensesId).LicenseStatus.ToString();
            var response = new GetLicensesResponseDto
            {
                ProductName = ProductName,
                ExpireDate = Licenses.ExpireDate,
                licenseStatus = LicenseStatus,
            };
            return response;
        }
    }
}
