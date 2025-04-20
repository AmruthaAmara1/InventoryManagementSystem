using InventoryManagementSystem.Models;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace InventoryManagementSystem.Helpers
{
    public static class FileExporter
    {
        public static void ExportSalesCsvWithDetails(Report summary, List<Transaction> transactions)
        {
            var dialog = new SaveFileDialog { FileName = "SalesReport.csv", Filter = "CSV Files|*.csv" };
            if (dialog.ShowDialog() != true) return;

            using var writer = new StreamWriter(dialog.FileName);
            writer.WriteLine($"Report Type:, {summary.ReportType}");
            writer.WriteLine($"Generated Date:, {summary.GeneratedDate}");
            writer.WriteLine($"Total Sales:, {summary.TotalSales}");
            writer.WriteLine($"Total Revenue:, {summary.TotalRevenue}");
            writer.WriteLine($"Total Products Sold:, {summary.TotalProductsSold}");
            writer.WriteLine($"Total Profit:, {summary.TotalProfit}");
            writer.WriteLine();
            writer.WriteLine("Date,Product,Quantity,Cost Price,Selling Price,Total,Profit");

            foreach (var t in transactions)
            {
                writer.WriteLine($"{t.Date},{t.Product.Name},{t.Quantity},{t.CostPrice},{t.SellingPrice},{t.SellingPrice * t.Quantity},{t.Profit}");
            }
        }

        public static void ExportProductCsvWithDetails(Report summary, List<Product> products, string fileName)
        {
            var dialog = new SaveFileDialog { FileName = fileName, Filter = "CSV Files|*.csv" };
            if (dialog.ShowDialog() != true) return;

            using var writer = new StreamWriter(dialog.FileName);
            writer.WriteLine($"Report Type:, {summary.ReportType}");
            writer.WriteLine($"Generated Date:, {summary.GeneratedDate}");
            writer.WriteLine($"Total Products:, {summary.TotalSales}");
            writer.WriteLine($"Total Quantity:, {summary.TotalProductsSold}");
            writer.WriteLine();
            writer.WriteLine("ID,Name,Quantity");

            foreach (var p in products)
            {
                writer.WriteLine($"{p.ProductId},{p.Name},{p.Quantity}");
            }
        }

        public static void ExportTopSellingCsvWithDetails(Report summary, List<(string ProductName, int QuantitySold)> topSelling)
        {
            var dialog = new SaveFileDialog { FileName = "TopSellingReport.csv", Filter = "CSV Files|*.csv" };
            if (dialog.ShowDialog() != true) return;

            using var writer = new StreamWriter(dialog.FileName);
            writer.WriteLine($"Report Type:, {summary.ReportType}");
            writer.WriteLine($"Generated Date:, {summary.GeneratedDate}");
            writer.WriteLine($"Total Products Sold:, {summary.TotalProductsSold}");
            writer.WriteLine($"Total Sales:, {summary.TotalSales}");
            writer.WriteLine();
            writer.WriteLine("Product,Quantity Sold");

            foreach (var item in topSelling)
            {
                writer.WriteLine($"{item.ProductName},{item.QuantitySold}");
            }
        }
    }
}
