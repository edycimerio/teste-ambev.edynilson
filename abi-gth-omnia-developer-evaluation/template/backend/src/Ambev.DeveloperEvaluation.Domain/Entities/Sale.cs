using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale transaction in the system.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Sale : BaseEntity
{
    private const string NumberPrefix = "SALE-";
    private readonly List<SaleItem> _items = new();

    /// <summary>
    /// Gets the unique number of the sale.
    /// Automatically generated with format "SALE-yyyyMMddHHmmss".
    /// </summary>
    public string Number { get; private set; }

    /// <summary>
    /// Gets the date and time when the sale was created.
    /// </summary>
    public DateTime SaleDate { get; private set; }

    /// <summary>
    /// Gets the customer's name.
    /// Must not be null or empty.
    /// </summary>
    public string CustomerName { get; private set; }

    /// <summary>
    /// Gets the customer's document (e.g., CPF, CNPJ).
    /// Must not be null or empty.
    /// </summary>
    public string CustomerDocument { get; private set; }

    /// <summary>
    /// Gets the total amount of the sale after all discounts.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Gets whether the sale has been canceled.
    /// </summary>
    public bool IsCanceled { get; private set; }

    /// <summary>
    /// Gets the collection of items in this sale.
    /// </summary>
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    /// <param name="customerName">The name of the customer</param>
    /// <param name="customerDocument">The customer's document</param>
    /// <exception cref="ArgumentException">Thrown when any parameter is invalid</exception>
    public Sale(string customerName, string customerDocument)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("Customer name is required", nameof(customerName));

        if (string.IsNullOrWhiteSpace(customerDocument))
            throw new ArgumentException("Customer document is required", nameof(customerDocument));

        Number = $"{NumberPrefix}{DateTime.UtcNow:yyyyMMddHHmmss}";
        SaleDate = DateTime.UtcNow;
        CustomerName = customerName;
        CustomerDocument = customerDocument;
        TotalAmount = 0;
        IsCanceled = false;
    }

    /// <summary>
    /// Adds a new item to the sale.
    /// </summary>
    /// <param name="productName">The name of the product</param>
    /// <param name="productCode">The unique code of the product</param>
    /// <param name="unitPrice">The unit price of the product</param>
    /// <param name="quantity">The quantity of items</param>
    /// <exception cref="InvalidOperationException">Thrown when the sale is canceled or the product already exists</exception>
    /// <exception cref="ArgumentException">Thrown when any parameter is invalid</exception>
    public void AddItem(string productName, string productCode, decimal unitPrice, int quantity)
    {
        if (IsCanceled)
            throw new InvalidOperationException("Cannot add items to a canceled sale");

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required", nameof(productName));

        if (string.IsNullOrWhiteSpace(productCode))
            throw new ArgumentException("Product code is required", nameof(productCode));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        if (quantity > 20)
            throw new ArgumentException("Cannot sell more than 20 items of the same product", nameof(quantity));

        if (_items.Any(i => i.ProductCode == productCode))
            throw new InvalidOperationException($"Product {productCode} already exists in this sale");

        var item = new SaleItem(productName, productCode, unitPrice, quantity);
        _items.Add(item);
        RecalculateTotalAmount();
    }

    /// <summary>
    /// Updates all items in the sale.
    /// </summary>
    /// <param name="newItems">The new collection of items</param>
    /// <exception cref="InvalidOperationException">Thrown when the sale is canceled or there are duplicate products</exception>
    /// <exception cref="ArgumentException">Thrown when the items collection is invalid</exception>
    public void UpdateItems(IReadOnlyCollection<SaleItem> newItems)
    {
        if (IsCanceled)
            throw new InvalidOperationException("Cannot update items of a canceled sale");

        if (newItems == null || !newItems.Any())
            throw new ArgumentException("At least one item is required", nameof(newItems));

        var duplicateProducts = newItems.GroupBy(i => i.ProductCode)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateProducts.Any())
            throw new InvalidOperationException($"Duplicate products found: {string.Join(", ", duplicateProducts)}");

        foreach (var item in newItems)
        {
            if (item.Quantity > 20)
                throw new ArgumentException($"Cannot sell more than 20 items of the same product: {item.ProductName}", nameof(newItems));
        }

        _items.Clear();
        foreach (var item in newItems)
        {
            _items.Add(item);
        }
        RecalculateTotalAmount();
    }

    /// <summary>
    /// Removes an item from the sale.
    /// </summary>
    /// <param name="productCode">The unique code of the product to remove</param>
    /// <exception cref="InvalidOperationException">Thrown when the sale is canceled or the product is not found</exception>
    public void RemoveItem(string productCode)
    {
        if (IsCanceled)
            throw new InvalidOperationException("Cannot remove items from a canceled sale");

        var item = _items.FirstOrDefault(i => i.ProductCode == productCode);
        if (item == null)
            throw new InvalidOperationException("Product not found in the sale");

        _items.Remove(item);
        RecalculateTotalAmount();
    }

    /// <summary>
    /// Cancels the sale.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the sale is already canceled</exception>
    public void Cancel()
    {
        if (IsCanceled)
            throw new InvalidOperationException("Sale is already canceled");

        IsCanceled = true;
    }

    /// <summary>
    /// Recalculates the total amount of the sale based on all items.
    /// </summary>
    private void RecalculateTotalAmount()
    {
        TotalAmount = _items.Sum(i => i.TotalPrice);
    }

    /// <summary>
    /// Required by EF Core
    /// </summary>
    private Sale()
    {
        Number = string.Empty;
        CustomerName = string.Empty;
        CustomerDocument = string.Empty;
    }
}
