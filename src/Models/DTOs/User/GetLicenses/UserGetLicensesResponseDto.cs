using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.User.GetLicenses
{
    public class UserGetLicensesResponseDto
    {
        public string CompanyName { get; set; }
        public int LicensesStatus { get; set; }
    }
}
