namespace excel_to_analytics.Api.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public Classification Classification { get; set; }
        [MaxLength(100)]
        public int Demand { get; set; }
    }
    public enum Classification
    {
        firstClassification ,
        seccondCClassification,
        lastClassification
    }
}
