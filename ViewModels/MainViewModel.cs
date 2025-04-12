namespace InventoryManagementSystem.ViewModels
{
    internal class MainViewModel
    {
        public ProductViewModel ProductVM { get; set; }
        public TransactionViewModel TransactionVM { get; set; }
        public ReportViewModel ReportVM { get; set; }

        public MainViewModel()
        {
            ProductVM = new ProductViewModel();
            TransactionVM = new TransactionViewModel();
            ReportVM = new ReportViewModel();
        }
    }
}
