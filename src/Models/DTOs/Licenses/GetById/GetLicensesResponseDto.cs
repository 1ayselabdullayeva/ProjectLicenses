using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Licenses.GetById
{
    public class GetLicensesResponseDto
    {
        public string ProductName { get; set; }
        public DateTime ExpireDate { get; set; }
        public int UserCount { get; set; }
        public string licenseStatus { get; set; }

    }
}
