using InventoryManagement.Data;
using InventoryManagement.Models;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

public class InventoryViewModel : INotifyPropertyChanged
{
    private readonly InventoryContext _context;
    private InventoryItem _selectedItem;
    private string _searchText;

    public ObservableCollection<InventoryItem> Items { get; set; }
    public ICommand AddItemCommand { get; set; }
    public ICommand EditItemCommand { get; set; }
    public ICommand DeleteItemCommand { get; set; }
    public ICommand SearchCommand { get; set; }

    public InventoryViewModel()
    {
        _context = new InventoryContext();
        Items = new ObservableCollection<InventoryItem>();
        LoadItems();

        AddItemCommand = new RelayCommand(AddItem);
        SearchCommand = new RelayCommand(SearchItems);
    }

    public InventoryItem SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged(nameof(SelectedItem));
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged(nameof(SearchText));
        }
    }

    public void LoadItems()
    {
        //Items = new ObservableCollection<InventoryItem>(_context.InventoryItems.ToList());
        Items.Clear();
        var latest = _context.InventoryItems.ToList();
        foreach (var i in latest)
            Items.Add(i);
    }

    public void AddItem()
    {
        var newItem = new InventoryItem { Name = "New Item", StockQuantity = 0, Category = "Default Category", LastUpdated = DateTime.Now };
        _context.InventoryItems.Add(newItem);
        _context.SaveChanges();
        LoadItems();
    }

    public void AddCustomItem(InventoryItem item)
    {
        var newItem = new InventoryItem { Name = "New Item", StockQuantity = 0, Category = "Default Category", LastUpdated = DateTime.Now };
        newItem.Name = item.Name;
        newItem.StockQuantity = item.StockQuantity;
        newItem.Category = item.Category;
        newItem.LastUpdated = DateTime.Now;
        _context.InventoryItems.Add(newItem);
        _context.SaveChanges();
        LoadItems();
    }

    public void EditItem(InventoryItem updatedItem)
    {
        if (updatedItem != null)
        {
            updatedItem.LastUpdated = DateTime.Now;
            _context.InventoryItems.Update(updatedItem);
            _context.SaveChanges();
            LoadItems();
        }
    }

    public void DeleteItem(bool confirm)
    {
        if (!confirm || SelectedItem == null) return;

        var itemToDelete = _context.InventoryItems.Find(SelectedItem.Id);
        if (itemToDelete != null)
        {
            _context.InventoryItems.Remove(itemToDelete);
            _context.SaveChanges();
            LoadItems();
        }
    }


    public void SearchItems()
    {
        var results = _context.InventoryItems
                .Where(item => item.Name.Contains(SearchText) || SearchText.IsNullOrEmpty()).ToList();
        Items = new ObservableCollection<InventoryItem>(results);
        OnPropertyChanged(nameof(Items));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool CanEditOrDelete() => SelectedItem != null;
}