﻿using Models.Enums;

namespace Models.DTOs.Licenses.Update
{
    public class LicensesUpdateStatusDto
    {
        public int Id { get; set; }
        public LiscenseStatus LicensesStatus { get; set; }
        public int UserId;
    }
}
