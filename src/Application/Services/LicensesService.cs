using Core.Repositories.Specific;
using Core.Services;

namespace Business.Services
{
	public class LicensesService : ILicensesServices
	{
		private readonly ILicensesRepository _licensesRepository;

        public LicensesService(ILicensesRepository licensesRepository)
        {
            _licensesRepository = licensesRepository;
        }
    }
}
