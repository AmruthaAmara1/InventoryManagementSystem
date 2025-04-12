using InventoryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Services
{
    public class TransactionService
    {
        private readonly DatabaseContext _context;

        public TransactionService()
        {
            _context = new DatabaseContext();
        }

        // Delegate and Event Definitions
        public delegate void TransactionChangedEventHandler(Transaction transaction);
        public event TransactionChangedEventHandler TransactionAdded;
        public event TransactionChangedEventHandler TransactionDeleted;

        public void AddTransaction(Transaction transaction)
        {
            var product = _context.Products.Find(transaction.ProductId);

            if (product == null)
                throw new InvalidOperationException("The product does not exist.");

            // Validate quantity if it's a sale
            if (transaction.Type == "Sale" && transaction.Quantity > product.Quantity)
                throw new InvalidOperationException("Insufficient stock for the transaction.");

            // Attach product to avoid tracking conflicts
            _context.Entry(product).State = EntityState.Unchanged;
            transaction.Product = product;

            _context.Transactions.Add(transaction);

            // Adjust stock
            if (transaction.Type == "Sale")
            {
                product.Quantity -= transaction.Quantity;
            }
            else if (transaction.Type == "Purchase")
            {
                product.Quantity += transaction.Quantity;
            }

            _context.Entry(product).State = EntityState.Modified;

            _context.SaveChanges();

            TransactionAdded?.Invoke(transaction);
        }

        public void DeleteTransaction(int transactionId)
        {
            var transaction = _context.Transactions.Find(transactionId);
            if (transaction != null)
            {
                var product = _context.Products.Find(transaction.ProductId);
                if (product != null)
                {
                    if (transaction.Type == "Sale")
                        product.Quantity += transaction.Quantity;
                    else if (transaction.Type == "Purchase")
                        product.Quantity -= transaction.Quantity;

                    _context.Entry(product).State = EntityState.Modified;
                    _context.SaveChanges();
                    _context.Entry(product).Reload();
                }

                _context.Transactions.Remove(transaction);
                _context.SaveChanges();
                TransactionDeleted?.Invoke(transaction);
            }
        }

        public List<Transaction> GetAllTransactions()
        {
            return _context.Transactions.Include(t => t.Product).ToList();
        }
    }
}
