using InventoryManagementSystem.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace InventoryManagementSystem.Views
{
    public partial class ReportView : UserControl
    {
        private ReportViewModel _viewModel;

        public ReportView()
        {
            InitializeComponent();
            _viewModel = (ReportViewModel)DataContext;
        }

        private async void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (ReportTypeComboBox.SelectedItem is ComboBoxItem selected)
            {
                string reportType = selected.Content.ToString();
                _viewModel.FromDate = FromDatePicker.SelectedDate;
                _viewModel.ToDate = ToDatePicker.SelectedDate;

                await _viewModel.GenerateReportAsync(reportType);
            }
        }

        private void ExportCsv_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Export("CSV");
        }
        
        private void ReportTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReportTypeComboBox.SelectedItem is ComboBoxItem selected)
            {
                string type = selected.Content.ToString();
                bool enableDates = (type == "Sales" || type == "Top Selling");

                FromDatePicker.IsEnabled = enableDates;
                ToDatePicker.IsEnabled = enableDates;
            }
        }

        private async void ReportView_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadReportsAsync();
        }

    }
}
