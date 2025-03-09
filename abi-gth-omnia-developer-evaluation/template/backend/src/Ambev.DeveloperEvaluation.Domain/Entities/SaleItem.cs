using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item in a sale with product information and pricing details.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets the name of the product.
    /// Must not be null or empty.
    /// </summary>
    public string ProductName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the unique code of the product.
    /// Must not be null or empty and should be unique within a sale.
    /// </summary>
    public string ProductCode { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the quantity of items.
    /// Must be between 1 and 20 units.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets the unit price of the product.
    /// Must be greater than zero.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Gets the discount applied to this item based on quantity rules.
    /// - 20% discount for quantities between 10 and 20
    /// - 10% discount for quantities between 4 and 9
    /// - No discount for quantities less than 4
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Gets the total price after applying the discount.
    /// Calculated as: (UnitPrice * Quantity) - Discount
    /// </summary>
    public decimal TotalPrice { get; private set; }

    /// <summary>
    /// Initializes a new instance of the SaleItem class.
    /// </summary>
    /// <param name="productName">The name of the product</param>
    /// <param name="productCode">The unique code of the product</param>
    /// <param name="unitPrice">The unit price of the product</param>
    /// <param name="quantity">The quantity of items</param>
    /// <exception cref="ArgumentException">Thrown when any parameter is invalid</exception>
    public SaleItem(string productName, string productCode, decimal unitPrice, int quantity)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required", nameof(productName));
        
        if (string.IsNullOrWhiteSpace(productCode))
            throw new ArgumentException("Product code is required", nameof(productCode));
        
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        if (quantity > 20)
            throw new ArgumentException("Maximum quantity per item is 20", nameof(quantity));
        
        if (unitPrice <= 0)
            throw new ArgumentException("Unit price must be greater than zero", nameof(unitPrice));

        ProductName = productName;
        ProductCode = productCode;
        UnitPrice = unitPrice;
        Quantity = quantity;

        CalculateDiscount();
        CalculateTotalPrice();
    }

    /// <summary>
    /// Calculates the discount based on quantity rules:
    /// - 20% for quantities between 10 and 20
    /// - 10% for quantities between 4 and 9
    /// - No discount for quantities less than 4
    /// </summary>
    private void CalculateDiscount()
    {
        decimal discountPercentage = 0;

        if (Quantity >= 10 && Quantity <= 20)
            discountPercentage = 0.20m; // 20% discount
        else if (Quantity >= 4)
            discountPercentage = 0.10m; // 10% discount

        Discount = UnitPrice * Quantity * discountPercentage;
    }

    /// <summary>
    /// Calculates the total price after applying the discount.
    /// </summary>
    private void CalculateTotalPrice()
    {
        TotalPrice = (UnitPrice * Quantity) - Discount;
    }

    /// <summary>
    /// Required by EF Core
    /// </summary>
    private SaleItem()
    {
        ProductName = string.Empty;
        ProductCode = string.Empty;
    }
}
