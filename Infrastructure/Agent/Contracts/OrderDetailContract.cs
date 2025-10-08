namespace Infrastructure.Contracts
{
    public class OrderDetailContract
    {
        public int Id { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string? DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItemContract> OrderItems { get; set; }
        public decimal Total { get; set; }
        public string? Status { get; set; }
    }
}