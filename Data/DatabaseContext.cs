using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace InventoryManagementSystem.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inventory.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
