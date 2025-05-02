namespace Room2.Models
{
    public class Bill
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
