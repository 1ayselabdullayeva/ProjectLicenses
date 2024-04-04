namespace Models.DTOs.Licenses.GetById
{
    public class GetLicensesResponseDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public DateTime ExpireDate { get; set; }
        public int UserCount { get; set; }
        public string licenseStatus { get; set; }

    }
}
