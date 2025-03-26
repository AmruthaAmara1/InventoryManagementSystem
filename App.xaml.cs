using System;
using System.Windows;
using InventoryManagementSystem.Data;

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
