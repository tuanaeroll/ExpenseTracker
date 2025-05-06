namespace ExpenseTracker.Business.Dtos.User
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } 
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public string Role { get; set; }
    }
}
