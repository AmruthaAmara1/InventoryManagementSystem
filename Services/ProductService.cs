using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Services
{
    public class ProductService
    {
        private readonly DatabaseContext _context;

        public ProductService()
        {
            _context = new DatabaseContext();
        }

        // Delegate and Event Definitions
        public delegate void ProductChangedEventHandler(Product product);
        public event ProductChangedEventHandler ProductAdded;
        public event ProductChangedEventHandler ProductUpdated;
        public event ProductChangedEventHandler ProductDeleted;

        // Add Product
        public void AddProduct(Product product)
        {
            // Check if the product with the same name and price already exists
            var existingProduct = _context.Products
                .Where(p => p.Name.ToLower() == product.Name.ToLower() && p.Price == product.Price)
                .FirstOrDefault();

            if (existingProduct != null)
            {
                throw new InvalidOperationException("A product with the same name and price already exists.");
            }

            _context.Products.Add(product);
            _context.SaveChangesAsync();
            ProductAdded?.Invoke(product); // Raise Event
        }

        // Update Product
        public async void UpdateProduct(Product product)
        {
            var existingProduct = _context.Products.Find(product.ProductId);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Quantity = product.Quantity;
            }
        }

        // Delete Product
        public void DeleteProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                ProductDeleted?.Invoke(product); // Raise Event
            }
        }

        // Get all Products
        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == productId);
        }
    }
}
