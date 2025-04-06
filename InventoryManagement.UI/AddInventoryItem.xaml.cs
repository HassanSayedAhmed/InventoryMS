using InventoryManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InventoryManagement.UI
{
    public partial class AddInventoryItem : Window
    {
        private InventoryItem _item;
        private InventoryViewModel _itemVM;

        public AddInventoryItem(InventoryViewModel itemVM)
        {
            InitializeComponent();
            _itemVM = itemVM;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateFields())
            {
                var newItem = new InventoryItem
                {
                    Name = NameTextBox.Text,
                    StockQuantity = int.Parse(StockQuantityTextBox.Text),
                    Category = CategoryTextBox.Text
                };
                _itemVM.AddCustomItem(newItem);

                this.DialogResult = true;
                Close();
            }

        }

        private bool ValidateFields()
        {
            var errorBuilder = new StringBuilder();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                errorBuilder.AppendLine("Name is required.");
                isValid = false;
            }

            if (!int.TryParse(StockQuantityTextBox.Text, out int quantity))
            {
                errorBuilder.AppendLine("Stock Quantity must be a valid number.");
                isValid = false;
            }
            else if (quantity < 0)
            {
                errorBuilder.AppendLine("Stock Quantity must not be negative.");
                isValid = false;
            }

            if (!isValid)
            {
                MessageBox.Show(errorBuilder.ToString(), "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return isValid;
        }
    }
}
