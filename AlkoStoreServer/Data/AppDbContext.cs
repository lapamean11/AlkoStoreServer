using AlkoStoreServer.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using System.Reflection;

namespace AlkoStoreServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Store> Store { get; set; }

        public DbSet<ProductStore> ProductStore { get; set; }

        public DbSet<ProductCategory> ProductCategory { get; set; }

        public DbSet<AttributeType> AttributeType { get; set; }

        public DbSet<AlkoStoreServer.Models.CategoryAttribute> CategoryAttribute { get; set; }

        public DbSet<ProductAttribute> ProductAttribute { get; set; }

        public DbSet<CategoryAttributeCategory> CategoryAttributeCategory { get; set; }

        public DbSet<ProductAttributeProduct> ProductAttributeProduct { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<AdminUser> AdminUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity => {

                entity.ToTable("Product");
            });

            modelBuilder.Entity<AdminUser>(entity => {

                entity.HasOne(au => au.Role)
                    .WithMany(r => r.AdminUsers)
                    .HasForeignKey(au => au.RoleId);

                entity.ToTable("AdminUser");
            });

            modelBuilder.Entity<ProductAttribute>(entity => {

                entity.HasOne(pa => pa.AttributeType)
                    .WithMany(at => at.ProductAttribute)
                    .HasForeignKey(pa => pa.TypeId);

                entity.ToTable("ProductAttribute");
            });

            modelBuilder.Entity<AlkoStoreServer.Models.CategoryAttribute>(entity => {

                entity.HasOne(pa => pa.AttributeType)
                    .WithMany(at => at.CategoryAttribute)
                    .HasForeignKey(pa => pa.TypeId);

                entity.ToTable("CategoryAttribute");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasOne(pc => pc.Product)
                        .WithMany(p => p.Categories)
                        .HasForeignKey(pc => pc.ProductId);

                entity.HasOne(pc => pc.Category)
                        .WithMany(c => c.Products)
                        .HasForeignKey(pc => pc.CategoryId);

                entity.ToTable("ProductCategory");
            });

            modelBuilder.Entity<Category>(entity => {

                /*entity.HasMany(c => c.Products)
                        .WithMany(p => p.Categories)
                            .UsingEntity<Dictionary<string, object>>(

                        "ProductCategory",
                        j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
                        j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId")
                    );*/

                entity.HasOne(e => e.ParentCategory)
                    .WithMany(e => e.ChildCategories)
                    .HasForeignKey(e => e.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable("Category");
            });

            modelBuilder.Entity<CategoryAttributeCategory>(entity => {

                entity.HasOne(cac => cac.Category)
                        .WithMany(c => c.CategoryAttributes)
                        .HasForeignKey(cac => cac.CategoryId);

                entity.HasOne(cac => cac.Attribute)
                        .WithMany(ca => ca.Categories)
                        .HasForeignKey(cac => cac.AttributeId);

                entity.ToTable("CategoryAttributeCategory");
            });

            modelBuilder.Entity<ProductAttributeProduct>(entity => {

                entity.HasOne(cac => cac.Product)
                        .WithMany(c => c.ProductAttributes)
                        .HasForeignKey(cac => cac.ProductId);

                entity.HasOne(cac => cac.Attribute)
                        .WithMany(ca => ca.Products)
                        .HasForeignKey(cac => cac.AttributeId);

                entity.ToTable("ProductAttributeProduct");
            });

            modelBuilder.Entity<ProductStore>(entity => {

                entity.HasOne(ps => ps.Product)
                        .WithMany(p => p.ProductStore)
                        .HasForeignKey(ps => ps.ProductId);

                entity.HasOne(ps => ps.Store)
                        .WithMany(s => s.ProductStore)
                        .HasForeignKey(ps => ps.StoreId);

                entity.ToTable("Product_Store");
            });

            modelBuilder.Entity<Review>(entity => {

                entity.HasOne(r => r.Product)
                        .WithMany(p => p.Reviews)
                        .HasForeignKey(r => r.ProductId);

                entity.HasOne(r => r.User)
                        .WithMany(u => u.Reviews)
                        .HasForeignKey(r => r.UserId);

                entity.ToTable("Review");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
