namespace Models.DTOs.Licenses.GetAll
{
    public class LicensesGetAllResponseDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public DateTime ActivationDate {  get; set; }
        public DateTime ExpireDate { get; set; }
        public int UserCount { get; set; }
        public string licenseStatus { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
    }
}
