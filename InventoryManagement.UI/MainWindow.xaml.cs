using InventoryManagement.Data;
using InventoryManagement.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InventoryManagement.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new InventoryViewModel();
    }

    private void AddItem_Click(object sender, RoutedEventArgs e)
    {
        var item = (InventoryViewModel)DataContext;
        var addItemWindow = new AddInventoryItem(item);
        addItemWindow.ShowDialog();
    }
    private void EditItem_Click(object sender, RoutedEventArgs e)
    {
        var vm = DataContext as InventoryViewModel;
        if (vm?.SelectedItem != null)
        {
            var editWindow = new EditInventoryItem(vm.SelectedItem, vm);
            editWindow.ShowDialog();
        }
        else
        {
            MessageBox.Show("Please select an item to edit.");
        }
    }
    private void DeleteItem_Click(object sender, RoutedEventArgs e)
    {
        var vm = DataContext as InventoryViewModel;

        if (vm?.SelectedItem == null)
        {
            MessageBox.Show("Please select an item to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show($"Are you sure you want to delete '{vm.SelectedItem.Name}'?",
                                     "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            vm.DeleteItem(true);
        }
    }

    private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
        var viewModel = DataContext as InventoryViewModel;

        if (e.EditAction == DataGridEditAction.Commit)
        {
            var editedItem = e.Row.Item as InventoryItem;

            if (editedItem != null)
            {
                if (ValidateFields(editedItem))
                {
                    if (editedItem.Id == 0)
                    {
                        viewModel.AddCustomItem(editedItem);
                    }
                    else
                    {
                        viewModel.EditItem(editedItem);
                    }

                    viewModel.LoadItems();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }


    private bool ValidateFields(InventoryItem item)
    {
        var errorBuilder = new StringBuilder();
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(item.Name))
        {
            errorBuilder.AppendLine("Name is required.");
            isValid = false;
        }

        if (item.StockQuantity < 0)
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