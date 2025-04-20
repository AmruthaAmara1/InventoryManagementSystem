using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Services
{
    public interface IReportGenerator
    {
        Task<Report> GenerateAsync();
    }
}