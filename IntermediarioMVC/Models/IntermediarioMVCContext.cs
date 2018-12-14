using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace IntermediarioMVC.Models
{
    public class IntermediarioMVCContext : DbContext
    {
        public IntermediarioMVCContext() : base("DefaultConnection")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Provider> Providers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<ProductInStock> ProductInStocks { get; set; }
        public DbSet<ChangeState> ChangeStates { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Pay> Pays { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Messenger> Messengers { get; set; }

    }
}