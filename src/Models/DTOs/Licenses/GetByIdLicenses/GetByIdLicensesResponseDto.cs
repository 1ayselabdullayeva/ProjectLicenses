using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Licenses.GetByIdLicenses
{
    public class GetByIdLicensesResponseDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int UsersCount {  get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string LiscenseStatus { get; set; }
    }
}
