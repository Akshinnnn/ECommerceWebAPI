using Data.DAL.ExpressionHelper;
using Data.Entities;
using Data.Entities.PropertyInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAL
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Entity type configurations
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            //Setting global query filter for soft deleted entities
            var entities = typeof(ISoftDelete).Assembly.GetTypes()
                .Where(t => typeof(ISoftDelete).IsAssignableFrom(t) 
                && t.IsClass);

            foreach (var entity in entities)
            {
                builder.Entity(entity)
                    .HasQueryFilter(GlobalQueryExpressionGenerator.GenerateExpression(entity));
            }

            //OrderProduct configuration
            builder.Entity<OrderProduct>().HasKey(x => new { x.OrderId, x.ProductId});

            base.OnModelCreating(builder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductInformation> ProductInformations { get; set; }
        public DbSet<ProductionCompany> ProductionCompanies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
    }
}
