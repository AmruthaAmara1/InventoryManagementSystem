using InventoryManagementSystem.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Transaction : INotifyPropertyChanged
{
    private int _quantity;
    private DateTime _date;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionId { get; set; }

    public int ProductId { get; set; }

    public int Quantity
    {
        get => _quantity;
        set { _quantity = value; OnPropertyChanged(nameof(Quantity)); }
    }

    public DateTime Date
    {
        get => _date;
        set { _date = value; OnPropertyChanged(nameof(Date)); }
    }

    public string Type { get; set; } // "Sale" or "Purchase"

    public decimal CostPrice { get; set; }

    public decimal SellingPrice { get; set; }

    [NotMapped]
    public decimal Revenue => Type == "Sale" ? SellingPrice * Quantity : 0;

    [NotMapped]
    public decimal TotalCost => Type == "Purchase" ? CostPrice * Quantity : 0;

    [NotMapped]
    public decimal Profit => Type == "Sale" ? (SellingPrice - CostPrice) * Quantity : 0;

    public virtual Product Product { get; set; }

    public decimal TotalPrice => SellingPrice * Quantity;

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
