using InventoryManagementSystem.Data;
using InventoryManagementSystem.Helpers;
using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace InventoryManagementSystem.Services
{
    public class SalesReportGenerator : IReportGenerator
    {
        private readonly DatabaseContext _context;

        public SalesReportGenerator()
        {
            _context = new DatabaseContext();
        }

        public async Task<Report> GenerateAsync()
        {
            var transactions = await _context.Transactions.ToListAsync();

            return new Report
            {
                GeneratedDate = DateTime.Now,
                ReportType = "Sales",
                TotalSales = transactions.Count(t => t.Type == "Sale"),
                TotalRevenue = transactions.Where(t => t.Type == "Sale").Sum(t => t.SellingPrice * t.Quantity),
                TotalProductsSold = transactions.Where(t => t.Type == "Sale").Sum(t => t.Quantity),
                TotalProfit = transactions.Where(t => t.Type == "Sale").Sum(t => t.Profit)
            };
        }
    }
}
