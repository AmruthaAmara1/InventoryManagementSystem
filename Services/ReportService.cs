using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Services
{
    public class ReportService
    {
        private readonly DatabaseContext _context = new DatabaseContext();

        public async Task<Report> GenerateReportAsync(string type, DateTime? from, DateTime? to)
        {
            switch (type)
            {
                case "Sales": return await GenerateSalesReport(from, to);
                case "Inventory": return await GenerateInventoryReport();
                case "Low Stock": return await GenerateLowStockReport();
                case "Top Selling": return await GenerateTopSellingReport(from, to);
                default: throw new ArgumentException("Invalid report type");
            }
        }

        public async Task<List<Transaction>> GetSalesTransactionsAsync(DateTime? from, DateTime? to)
        {
            var query = _context.Transactions.Include(t => t.Product).Where(t => t.Type == "Sale");
            if (from.HasValue) query = query.Where(t => t.Date >= from.Value);
            if (to.HasValue) query = query.Where(t => t.Date <= to.Value);
            return await query.ToListAsync();
        }

        public async Task<List<Product>> GetInventoryProductsAsync() =>
            await _context.Products.ToListAsync();

        public async Task<List<Product>> GetLowStockProductsAsync() =>
            await _context.Products.Where(p => p.Quantity < 5).ToListAsync();

        public async Task<List<(string ProductName, int QuantitySold)>> GetTopSellingProductsAsync(DateTime? from, DateTime? to)
        {
            var query = _context.Transactions.Include(t => t.Product).Where(t => t.Type == "Sale");
            if (from.HasValue) query = query.Where(t => t.Date >= from.Value);
            if (to.HasValue) query = query.Where(t => t.Date <= to.Value);

            var result = await query
                .GroupBy(t => t.Product.Name)
                .Select(g => new
                {
                    ProductName = g.Key,
                    QuantitySold = g.Sum(t => t.Quantity)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(10)
                .ToListAsync();

            return result.Select(x => (x.ProductName, x.QuantitySold)).ToList();
        }

        private async Task<Report> GenerateSalesReport(DateTime? from, DateTime? to)
        {
            var transactions = await GetSalesTransactionsAsync(from, to);
            var report = new Report
            {
                ReportType = "Sales",
                GeneratedDate = DateTime.Now,
                TotalSales = transactions.Count,
                TotalRevenue = transactions.Sum(t => t.SellingPrice * t.Quantity),
                TotalProductsSold = transactions.Sum(t => t.Quantity),
                TotalProfit = transactions.Sum(t => t.Profit)
            };
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        private async Task<Report> GenerateInventoryReport()
        {
            var products = await _context.Products.ToListAsync();
            var report = new Report
            {
                ReportType = "Inventory",
                GeneratedDate = DateTime.Now,
                TotalProductsSold = products.Sum(p => p.Quantity)
            };
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        private async Task<Report> GenerateLowStockReport()
        {
            var lowStock = await _context.Products.Where(p => p.Quantity < 5).ToListAsync();
            var report = new Report
            {
                ReportType = "Low Stock",
                GeneratedDate = DateTime.Now,
                TotalProductsSold = lowStock.Sum(p => p.Quantity)
            };
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        private async Task<Report> GenerateTopSellingReport(DateTime? from, DateTime? to)
        {
            var top = await GetTopSellingProductsAsync(from, to);
            var report = new Report
            {
                ReportType = "Top Selling",
                GeneratedDate = DateTime.Now,
                TotalProductsSold = top.Sum(p => p.QuantitySold),
                TotalSales = top.Count
            };
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<List<Report>> GetSavedReportsAsync()
        {
            return await _context.Reports.OrderByDescending(r => r.GeneratedDate).ToListAsync();
        }

    }
}
