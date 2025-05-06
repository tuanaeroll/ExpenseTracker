namespace ExpenseTracker.Business.Dtos.User
{
    public class RegisterUserRequestDto
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
        public string Position { get; set; }

        public string IBAN { get; set; } 
    }
}
