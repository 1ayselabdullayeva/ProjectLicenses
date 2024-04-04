namespace Models.DTOs.Licenses.Create
{
    public class LicensesCreateResponseDto
    {
        public int ProductId { get; set; }
        public int UserCount { get; set; }
        public DateTime ExpireDate { get; set; }

    }
}
