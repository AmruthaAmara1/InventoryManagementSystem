using InventoryManagementSystem.Models;
using InventoryManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;

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
            _context.Products.Add(product);
            _context.SaveChanges();
            ProductAdded?.Invoke(product); // Raise Event
        }

        // Update Product
        public void UpdateProduct(Product product)
        {
            var existingProduct = _context.Products.Find(product.ProductId);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Quantity = product.Quantity;
                _context.SaveChanges();
                ProductUpdated?.Invoke(product); // Raise Event
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
    }
}
