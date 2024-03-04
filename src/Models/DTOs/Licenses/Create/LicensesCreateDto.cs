using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.DTOs.Licenses.Create
{
    public class LicensesCreateDto
    {
        public string ProductName { get; set; }
        public int UserCount { get; set; }
        public int Year { get; set; }
    }

}
