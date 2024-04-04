using Models.DTOs;
using Models.Entities;

namespace Core.Repositories.Specific
{
	public interface ILicensesRepository: IRepository<Licenses>
	{
         PagedList<Licenses> GetLicenses(PagedParameters licensesParameters);
        Licenses GetLicensesId(Licenses licensesId);
    }
}
