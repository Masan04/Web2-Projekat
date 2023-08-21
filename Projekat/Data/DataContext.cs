using Microsoft.EntityFrameworkCore;
using Projekat.Models;
using System.Reflection.Emit;

namespace Projekat.Data
{
    public class DataContext : DbContext
    { 
        public DataContext(DbContextOptions<DataContext>options) : base(options) { }

        public DbSet<Item> Items { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ItemOrder> ItemsInsideOrders { get; set; }

        public DbSet<Order> Orders { get; set; }  



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            //Kazemo mu da pronadje sve konfiguracije u Assembliju i da ih primeni nad bazom
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }

                    

    }
}
