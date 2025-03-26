using InventoryManagementSystem.Models;
using InventoryManagementSystem.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace InventoryManagementSystem.Views
{
    public partial class ProductView : UserControl
    {
        public ProductView()
        {
            InitializeComponent();
        }

        private void ProductDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem is Product selectedProduct)
            {
                // Populate textboxes with selected product details
                NameInput.Text = selectedProduct.Name;
                PriceInput.Text = selectedProduct.Price.ToString("F2");
                QuantityInput.Text = selectedProduct.Quantity.ToString();
            }
            else
            {
                // Clear textboxes, if no product is selected
                NameInput.Clear();
                PriceInput.Clear();
                QuantityInput.Clear();
            }
        }

        private void ClearFields_Click(object sender, RoutedEventArgs e)
        {
            // Clear all the input fields
            NameInput.Text = string.Empty;
            PriceInput.Text = string.Empty;
            QuantityInput.Text = string.Empty;
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (ProductViewModel)DataContext;

            var product = new Product
            {
                Name = NameInput.Text,
                Price = decimal.Parse(PriceInput.Text),
                Quantity = int.Parse(QuantityInput.Text)
            };
            viewModel.AddProduct(product);
        }

        private void UpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (ProductViewModel)DataContext;

            if (ProductDataGrid.SelectedItem is Product selectedProduct)
            {
                selectedProduct.Name = NameInput.Text;
                selectedProduct.Price = decimal.Parse(PriceInput.Text);
                selectedProduct.Quantity = int.Parse(QuantityInput.Text);

                viewModel.UpdateProduct(selectedProduct);
            }
            else
            {
                MessageBox.Show("Please select a product to update.");
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (ProductViewModel)DataContext;

            if (ProductDataGrid.SelectedItem is Product selectedProduct)
            {
                viewModel.DeleteProduct(selectedProduct.ProductId);
            }
            else
            {
                MessageBox.Show("Please select a product to delete.");
            }
        }
    }
}
