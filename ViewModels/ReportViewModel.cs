using InventoryManagementSystem.Commands;
using InventoryManagementSystem.Helpers;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Packaging;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InventoryManagementSystem.ViewModels
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        private readonly ReportService _reportService = new ReportService();
        private ObservableCollection<Report> _reports = new ObservableCollection<Report>();

        public ObservableCollection<Report> Reports
        {
            get => _reports;
            set { _reports = value; OnPropertyChanged(nameof(Reports)); }
        }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public async Task GenerateReportAsync(string type)
        {
            var report = await _reportService.GenerateReportAsync(type, FromDate, ToDate);
            Reports.Add(report);
        }

        public async void Export(string format)
        {
            if (Reports.Count == 0) return;

            var report = Reports.Last();

            switch (format)
            {
                case "CSV":
                    if (report.ReportType == "Sales")
                    {
                        var data = await _reportService.GetSalesTransactionsAsync(FromDate, ToDate);
                        FileExporter.ExportSalesCsvWithDetails(report, data);
                    }
                    else if (report.ReportType == "Inventory")
                    {
                        var data = await _reportService.GetInventoryProductsAsync();
                        FileExporter.ExportProductCsvWithDetails(report, data, "InventoryReport.csv");
                    }
                    else if (report.ReportType == "Low Stock")
                    {
                        var data = await _reportService.GetLowStockProductsAsync();
                        FileExporter.ExportProductCsvWithDetails(report, data, "LowStockReport.csv");
                    }
                    else if (report.ReportType == "Top Selling")
                    {
                        var data = await _reportService.GetTopSellingProductsAsync(FromDate, ToDate);
                        FileExporter.ExportTopSellingCsvWithDetails(report, data);
                    }
                    break;
            }
        }

        public async Task LoadReportsAsync()
        {
            var savedReports = await _reportService.GetSavedReportsAsync();
            Reports = new ObservableCollection<Report>(savedReports);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
