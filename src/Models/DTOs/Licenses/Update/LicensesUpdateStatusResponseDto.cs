using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Licenses.Update
{
    public class LicensesUpdateStatusResponseDto
    {
        public int Id { get; set; }
        public Models.Enums.LiscenseStatus LicensesStatus { get; set; }

        public int UserId;
    }
}
