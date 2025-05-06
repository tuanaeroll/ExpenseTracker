using ExpenseTracker.Data.Enums;

namespace ExpenseTracker.Data.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string IBAN { get; set; }
        public Role Role { get; set; }

        public string Position { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<Expense> Expenses { get; set; }
    }
}
