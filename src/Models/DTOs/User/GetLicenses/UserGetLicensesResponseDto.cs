using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.User.GetLicenses
{
    public class UserGetLicensesResponseDto
    {
        public string CompanyName { get; set; }
        public int TotalLicenses { get; set; }  
        public int ActiveLicensesCount { get; set; }
        public int LockedLicensesCount { get; set; }
        public int ExpiredLicensesCount { get; set; }
    }
}
