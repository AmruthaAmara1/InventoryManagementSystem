using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.ViewModels
{
    internal class MainViewModel
    {
        public ProductViewModel ProductVM { get; set; }

        public MainViewModel()
        {
            ProductVM = new ProductViewModel();
        }
    }
}
