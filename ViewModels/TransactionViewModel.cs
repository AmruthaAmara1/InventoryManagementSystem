using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace InventoryManagementSystem.ViewModels
{
    public class TransactionViewModel : INotifyPropertyChanged
    {
        private readonly TransactionService _transactionService;
        private readonly ProductService _productService;

        private ObservableCollection<Transaction> _transactions;
        private bool _isAddVisible = true;
        private Transaction _selectedTransaction;

        public ObservableCollection<string> TypeOptions { get; set; }

        public bool IsAddVisible
        {
            get => _isAddVisible;
            set
            {
                _isAddVisible = value;
                OnPropertyChanged(nameof(IsAddVisible));
            }
        }

        public Transaction SelectedTransaction
        {
            get => _selectedTransaction;
            set
            {
                if (_selectedTransaction != value)
                {
                    _selectedTransaction = value;
                    OnPropertyChanged(nameof(SelectedTransaction));
                    IsAddVisible = _selectedTransaction != null;
                }
            }
        }

        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set
            {
                _transactions = value;
                OnPropertyChanged(nameof(Transactions));
            }
        }

        public TransactionViewModel()
        {
            _transactionService = new TransactionService();
            _productService = new ProductService();
            _transactions = new ObservableCollection<Transaction>(_transactionService.GetAllTransactions());

            TypeOptions = new ObservableCollection<string> { "Sale", "Purchase" };

            _transactionService.TransactionAdded += OnTransactionAdded;
            _transactionService.TransactionDeleted += OnTransactionDeleted;
        }

        public void AddTransaction(Transaction transaction)
        {
            var product = _productService.GetProductById(transaction.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException("Invalid Product ID.");
            }

            transaction.Product = product;

            _transactionService.AddTransaction(transaction);
        }

        public Product GetProductById(int productId)
        {
            return _productService.GetProductById(productId);
        }

        // Event Handlers
        private void OnTransactionAdded(Transaction transaction) => Transactions.Add(transaction);
        private void OnTransactionDeleted(Transaction transaction) => Transactions.Remove(transaction);

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
