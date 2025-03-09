using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

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
    /// Gets or sets the unique number of the sale.
    /// Automatically generated with format "SALE-yyyyMMddHHmmss".
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the sale was created.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer's name.
    /// Must not be null or empty.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's document (e.g., CPF, CNPJ).
    /// Must not be null or empty.
    /// </summary>
    public string CustomerDocument { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total amount of the sale after all discounts.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets whether the sale has been canceled.
    /// </summary>
    public bool IsCanceled { get; set; }

    /// <summary>
    /// Gets or sets the collection of items in this sale.
    /// </summary>
    public List<SaleItem> Items { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        Number = $"{NumberPrefix}{DateTime.UtcNow:yyyyMMddHHmmss}";
        SaleDate = DateTime.UtcNow;
        TotalAmount = 0;
        IsCanceled = false;
    }

    /// <summary>
    /// Recalculates the total amount of the sale based on all items.
    /// </summary>
    public void RecalculateTotalAmount()
    {
        TotalAmount = Items.Sum(i => i.TotalPrice);
    }
}
