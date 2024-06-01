namespace excel_to_analytics.Api.databaseContext
{
    public partial class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(k => k.ProductId);
                entity.HasMany(p => p.Sales)
                      .WithOne(s => s.Product)
                      .HasForeignKey(s => s.ProductId);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(k => k.SaleId);
                entity.HasOne(s => s.Product)
                      .WithMany(p => p.Sales)
                      .HasForeignKey(s => s.ProductId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
