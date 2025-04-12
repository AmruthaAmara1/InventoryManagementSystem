using InventoryManagementSystem.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace InventoryManagementSystem.Views
{
    public partial class TransactionView : UserControl
    {
        private TransactionViewModel _transactionViewModel;

        public TransactionView()
        {
            InitializeComponent();
            _transactionViewModel = (TransactionViewModel)DataContext;
        }

        private void TransactionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransactionDataGrid.SelectedItem is Transaction selectedTransaction)
            {
                ProductIdInput.Text = selectedTransaction.ProductId.ToString();
                QuantityInput.Text = selectedTransaction.Quantity.ToString();
                TransactionDateInput.SelectedDate = selectedTransaction.Date;
                CostPriceInput.Text = selectedTransaction.CostPrice.ToString("F2");
                SellingPriceInput.Text = selectedTransaction.SellingPrice.ToString("F2");
                TransactionTypeComboBox.SelectedItem = selectedTransaction.Type;
            }
            else
            {
                ClearFields();
            }
        }

        private void ClearFields_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            ProductIdInput.Clear();
            QuantityInput.Clear();
            CostPriceInput.Clear();
            SellingPriceInput.Clear();
            TransactionDateInput.SelectedDate = null;
            TransactionTypeComboBox.SelectedItem = null;
        }

        private void ProductIdInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(ProductIdInput.Text, out int productId))
            {
                var product = ((TransactionViewModel)DataContext).GetProductById(productId);
                if (product != null)
                {
                    // Preload cost and selling prices with product default price
                    CostPriceInput.Text = product.Price.ToString("F2");
                }
                else
                {
                    CostPriceInput.Clear();
                    SellingPriceInput.Clear();
                }
            }
        }

        private void AddTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductIdInput.Text) ||
                string.IsNullOrWhiteSpace(QuantityInput.Text) ||
                string.IsNullOrWhiteSpace(CostPriceInput.Text) ||
                string.IsNullOrWhiteSpace(SellingPriceInput.Text) ||
                TransactionDateInput.SelectedDate == null ||
                TransactionTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(ProductIdInput.Text, out int productId) ||
                !int.TryParse(QuantityInput.Text, out int quantity) ||
                !decimal.TryParse(CostPriceInput.Text, out decimal costPrice) ||
                !decimal.TryParse(SellingPriceInput.Text, out decimal sellingPrice))
            {
                MessageBox.Show("Invalid number input", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var product = _transactionViewModel.GetProductById(productId);
            if (product == null)
            {
                MessageBox.Show("Invalid Product ID", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var transaction = new Transaction
            {
                ProductId = productId,
                Quantity = quantity,
                CostPrice = costPrice,
                SellingPrice = sellingPrice,
                Date = TransactionDateInput.SelectedDate.Value,
                Type = TransactionTypeComboBox.SelectedItem.ToString()
            };

            try
            {
                _transactionViewModel.AddTransaction(transaction);
                MessageBox.Show("Transaction added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add transaction:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TransactionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransactionTypeComboBox.SelectedItem is string selectedType &&
                int.TryParse(ProductIdInput.Text, out int productId))
            {
                var product = ((TransactionViewModel)DataContext).GetProductById(productId);
                if (product == null) return;

                // Reset fields
                CostPriceInput.Clear();
                SellingPriceInput.Clear();

                if (selectedType == "Purchase")
                {
                    // We're buying items
                    CostPriceInput.Text = product.Price.ToString("F2");
                    SellingPriceInput.IsEnabled = false;
                    SellingPriceInput.Text = "0.00";
                }
                else if (selectedType == "Sale")
                {
                    // We're selling items
                    CostPriceInput.Text = product.Price.ToString("F2");
                    SellingPriceInput.IsEnabled = true;
                }

            }
        }

    }
}
