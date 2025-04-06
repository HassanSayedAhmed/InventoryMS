using Moq;
using Xunit;
using InventoryManagement.Data;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

public class InventoryViewModelTests
{
    private Mock<InventoryContext> _mockContext;
    private InventoryViewModel _viewModel;
    private Mock<DbSet<InventoryItem>> _mockSet;

    public InventoryViewModelTests()
    {
        _mockContext = new Mock<InventoryContext>();
        _mockSet = new Mock<DbSet<InventoryItem>>();

        var items = new List<InventoryItem>
        {
            new InventoryItem { Id = 1, Name = "Item1", StockQuantity = 10, Category = "Category1", LastUpdated = DateTime.Now },
            new InventoryItem { Id = 2, Name = "Item2", StockQuantity = 20, Category = "Category2", LastUpdated = DateTime.Now }
        }.AsQueryable();

        _mockSet.As<IQueryable<InventoryItem>>().Setup(m => m.Provider).Returns(items.Provider);
        _mockSet.As<IQueryable<InventoryItem>>().Setup(m => m.Expression).Returns(items.Expression);
        _mockSet.As<IQueryable<InventoryItem>>().Setup(m => m.ElementType).Returns(items.ElementType);
        _mockSet.As<IQueryable<InventoryItem>>().Setup(m => m.GetEnumerator()).Returns(items.GetEnumerator());

        _mockContext.Setup(c => c.InventoryItems).Returns(_mockSet.Object);
        _viewModel = new InventoryViewModel();
    }

    [Fact]
    public void Test_AddItem()
    {
        // Act
        _viewModel.AddItem();

        // Assert that the item count has increased.
        _mockSet.Verify(m => m.Add(It.IsAny<InventoryItem>()), Times.Once); // Check if Add was called
        _mockContext.Verify(m => m.SaveChanges(), Times.Once); // Check if SaveChanges was called
    }

    [Fact]
    public void Test_DeleteItem()
    {
        // Arrange
        _viewModel.SelectedItem = _viewModel.Items.First();

        // Act
        _viewModel.DeleteItem(true);

        // Assert that the delete operation was called.
        _mockSet.Verify(m => m.Remove(It.IsAny<InventoryItem>()), Times.Once); // Check if Remove was called
        _mockContext.Verify(m => m.SaveChanges(), Times.Once); // Check if SaveChanges was called
    }

    [Fact]
    public void Test_EditItem()
    {
        // Arrange
        var itemToEdit = _viewModel.Items.First();
        itemToEdit.Name = "Updated Item";

        // Act
        _viewModel.EditItem(itemToEdit);

        // Assert that the update operation was called.
        _mockSet.Verify(m => m.Update(It.IsAny<InventoryItem>()), Times.Once); // Check if Update was called
        _mockContext.Verify(m => m.SaveChanges(), Times.Once); // Check if SaveChanges was called
    }

    [Fact]
    public void Test_SearchItems()
    {
        // Arrange
        _viewModel.SearchText = "Item1";

        // Act
        _viewModel.SearchItems();

        // Assert that the Items collection is filtered.
        Assert.Single(_viewModel.Items);
        Assert.Equal("Item1", _viewModel.Items.First().Name);
    }

    [Fact]
    public void Test_LoadItems()
    {
        // Act
        _viewModel.LoadItems();

        // Assert that the items are loaded correctly.
        Assert.Equal(2, _viewModel.Items.Count); // Assuming 2 items are loaded from the mock context
    }
}
