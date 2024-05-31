namespace excel_to_analytics.Api.databaseContext
{
    public partial class Context : DbContext
    {
        public Context(DbContextOptions
            <Context> options)
                : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(k => k.ProductId);
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
