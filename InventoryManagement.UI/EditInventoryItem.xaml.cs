using InventoryManagement.Data;
using System.Text;
using System.Windows;
namespace InventoryManagement.UI;

public partial class EditInventoryItem : Window
{
    private InventoryItem _item;
    private InventoryViewModel _viewModel;

    public EditInventoryItem(InventoryItem item, InventoryViewModel viewModel)
    {
        InitializeComponent();
        _item = item;
        _viewModel = viewModel;

        NameTextBox.Text = item.Name;
        CategoryTextBox.Text = item.Category;
        StockQuantityTextBox.Text = item.StockQuantity.ToString();
    }


    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (ValidateFields())
        {
            _item.Name = NameTextBox.Text;
            _item.Category = CategoryTextBox.Text;
            if (int.TryParse(StockQuantityTextBox.Text, out int quantity))
            {
                _item.StockQuantity = quantity;
                _viewModel.EditItem(_item);
                DialogResult = true;
                Close();
            }
        }
        else
        {
            MessageBox.Show("Stock Quantity must be a number.");
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