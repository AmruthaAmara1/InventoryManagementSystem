using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace InventoryManagementSystem.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly ProductService _productService;
        private ObservableCollection<Product> _products;
        private bool _isAddVisible = true;
        private Product _selectedProduct;

        public bool IsAddVisible
        {
            get { return _isAddVisible; }
            set
            {
                _isAddVisible = value;
                OnPropertyChanged(nameof(IsAddVisible));
            }
        }

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged(nameof(SelectedProduct));
                    IsAddVisible = _selectedProduct != null;
                }
            }
        }

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged(nameof(Products));
                }
            }
        }

        private void ProductDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Products != null && Products.Count > 0)
            {
                SelectedProduct = Products.FirstOrDefault();
                IsAddVisible = SelectedProduct != null; // To display the text fields only, if there is a selected product
            }
            else
            {
                SelectedProduct = null;
                IsAddVisible = false;
            }
        }

        public ProductViewModel()
        {
            _productService = new ProductService();
            _products = new ObservableCollection<Product>(_productService.GetAllProducts());

            _productService.ProductAdded += OnProductAdded;
            _productService.ProductUpdated += OnProductUpdated;
            _productService.ProductDeleted += OnProductDeleted;
        }

        public void AddProduct(Product product)
        {
            _productService.AddProduct(product);
        }

        public void UpdateProduct(Product product)
        {
            _productService.UpdateProduct(product);
        }

        public void DeleteProduct(int productId)
        {
            _productService.DeleteProduct(productId);
        }

        // Event Handlers to Update UI
        private void OnProductAdded(Product product) => Products.Add(product);

        private void OnProductUpdated(Product product)
        {
            var existing = Products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Price = product.Price;
                existing.Quantity = product.Quantity;
            }
        }

        private void OnProductDeleted(Product product) => Products.Remove(product);

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
