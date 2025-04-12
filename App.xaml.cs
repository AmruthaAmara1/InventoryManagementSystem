using InventoryManagementSystem.Data;
using System.Windows;

namespace InventoryManagementSystem
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Ensure database is created
            using (var context = new DatabaseContext())
            {
                context.Database.EnsureCreated(); // Creates DB & tables if missing
            }
        }
    }
}
