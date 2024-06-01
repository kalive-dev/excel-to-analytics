namespace excel_to_analytics.Api.Models
{
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public string Location { get; set; }
        public DateTime SaleDate { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public Product Product { get; set; }
    }
}