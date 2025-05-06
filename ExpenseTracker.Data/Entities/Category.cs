namespace ExpenseTracker.Data.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}
