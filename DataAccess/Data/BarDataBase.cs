using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Data
{
    public class BarDataBase : DbContext
    {
        public DbSet<MenuItem> Menu { get; set; }
        public DbSet<Order> Orders { get; set; }

        public BarDataBase () { }

        public BarDataBase(DbContextOptions<BarDataBase> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:mybarsql.database.windows.net,1433;Initial Catalog=BarDataBase;Persist Security Info=False;User ID=bar_admin;Password=C#isTheBest;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().Property(order => order.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Order>().Property(order => order.Status).IsRequired();
            modelBuilder.Entity<Order>().Property(order => order.TotalPrice).IsRequired();
            modelBuilder.Entity<MenuItem>().Property(menu => menu.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<MenuItem>().Property(menuItem => menuItem.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<MenuItem>().Property(menuItem => menuItem.Price).IsRequired();
        }
    }
}
