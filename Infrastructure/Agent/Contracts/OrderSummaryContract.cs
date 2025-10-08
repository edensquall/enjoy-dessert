namespace Infrastructure.Contracts
{
    public class OrderSummaryContract
    {
        public int Id { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public decimal Total { get; set; }
        public string? Status { get; set; }
    }
}