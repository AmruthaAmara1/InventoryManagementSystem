using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public string ReportType { get; set; }
        public DateTime GeneratedDate { get; set; }

        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalProductsSold { get; set; }
        public decimal TotalProfit { get; set; }
    }

}
