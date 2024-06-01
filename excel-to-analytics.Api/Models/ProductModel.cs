namespace excel_to_analytics.Api.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Classification { get; set; }

        public ICollection<Sale> Sales { get; set; }
    }
}
