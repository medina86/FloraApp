using Microsoft.EntityFrameworkCore;

namespace FloraApp.Services.Database
{
    public class FloraAppDbContext : DbContext
    {
        public FloraAppDbContext(DbContextOptions<FloraAppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<CustomBouquet> CustomBouquets { get; set; }
        public DbSet<CustomBouquetItem> CustomBouquetItems { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<DonationPayment> DonationPayments { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationProposal> ReservationProposals { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity relationships

            // User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
                
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
                
            // Product and Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
                
            // Category hierarchical relationship
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.Subcategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Asset
            modelBuilder.Entity<Asset>()
                .HasOne(a => a.Product)
                .WithMany(p => p.Assets)
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Cart
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany()
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // CartItem
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // CustomBouquetItem
            modelBuilder.Entity<CustomBouquetItem>()
                .HasOne(cbi => cbi.CustomBouquet)
                .WithMany()
                .HasForeignKey(cbi => cbi.CustomBouquetId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<CustomBouquetItem>()
                .HasOne(cbi => cbi.Product)
                .WithMany(p => p.CustomBouquetItems)
                .HasForeignKey(cbi => cbi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
                
            // BlogPost and BlogComment
            modelBuilder.Entity<BlogPost>()
                .HasMany(bp => bp.BlogComments)
                .WithOne(bc => bc.BlogPost)
                .HasForeignKey(bc => bc.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<BlogComment>()
                .HasOne(bc => bc.User)
                .WithMany()
                .HasForeignKey(bc => bc.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 