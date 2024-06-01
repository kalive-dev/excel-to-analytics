namespace excel_to_analytics.Api.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Classification { get; set; }
        public DateTime DateTimeofSell { get; set; }
        public int Price { get; set; }
        public string Address { get; set; }
    }
}
