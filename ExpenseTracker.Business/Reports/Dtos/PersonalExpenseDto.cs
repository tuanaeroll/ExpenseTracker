public class PersonalExpenseReportDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string CategoryName { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
    public DateTime CreatedAt { get; set; }

    public string CreatedAtFormatted => CreatedAt.ToString("yyyy-MM-dd HH:mm");
}
