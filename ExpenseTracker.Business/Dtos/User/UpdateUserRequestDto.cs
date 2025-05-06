namespace ExpenseTracker.Business.Dtos.User
{
    public class UpdateUserRequestDto
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Position { get; set; }
        public string? IBAN { get; set; }
    }

}
