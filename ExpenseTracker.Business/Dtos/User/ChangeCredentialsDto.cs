namespace ExpenseTracker.Business.Dtos.User
{
    public class ChangeCredentialsRequestDto
    {
        public string CurrentPassword { get; set; } 
        public string? NewEmail { get; set; }     
        public string? NewPassword { get; set; }  
    }
}
