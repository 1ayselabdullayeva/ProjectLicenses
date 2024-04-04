namespace Models.DTOs.Licenses.Update
{
    public class LicensesUpdateStatusResponseDto
    {
        public int Id { get; set; }
        public Models.Enums.LiscenseStatus LicensesStatus { get; set; }

        public int UserId;
    }
}
