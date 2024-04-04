namespace Models.DTOs.Licenses.Update
{
    public class LicensesUpdateStatusDto
    {
        public int Id { get; set; }
        public Models.Enums.LiscenseStatus LicensesStatus { get; set; }
        public int UserId;
    }
}
