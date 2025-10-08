namespace Infrastructure.Contracts
{
    public class OrderItemContract
    {
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}