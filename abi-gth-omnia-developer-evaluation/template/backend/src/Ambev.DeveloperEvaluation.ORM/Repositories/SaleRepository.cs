using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Repository implementation for managing sales using Entity Framework Core.
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task<Sale> GetByNumberAsync(string number, CancellationToken cancellationToken)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Number == number, cancellationToken);
    }

    public async Task<IEnumerable<Sale>> ListAsync(CancellationToken cancellationToken)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken)
    {
        var existingSale = await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Number == sale.Number, cancellationToken);

        if (existingSale == null)
            throw new KeyNotFoundException($"Sale with number {sale.Number} not found");

        if (existingSale.IsCanceled)
            throw new InvalidOperationException($"Sale {sale.Number} is canceled and cannot be updated");

        // Update sale properties
        existingSale.CustomerName = sale.CustomerName;
        existingSale.CustomerDocument = sale.CustomerDocument;

        // Remove existing items
        existingSale.Items.Clear();

        // Add new items
        foreach (var item in sale.Items)
        {
            var newItem = new SaleItem
            {
                ProductName = item.ProductName,
                ProductCode = item.ProductCode,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            };

            // Calculate discount based on quantity
            if (item.Quantity >= 10)
                newItem.Discount = 20; // 20% discount
            else if (item.Quantity >= 4)
                newItem.Discount = 10; // 10% discount

            newItem.RecalculateTotalPrice();
            existingSale.Items.Add(newItem);
        }

        existingSale.RecalculateTotalAmount();
        //try
        //{
            await _context.SaveChangesAsync(cancellationToken);
        //}
        //catch (Exception ex)
        //{
        //    string message = ex.Message;    
        //}
        return existingSale;
    }

    public async Task<bool> CancelAsync(string number, CancellationToken cancellationToken = default)
    {
        var sale = await GetByNumberAsync(number, cancellationToken);
        if (sale == null || sale.IsCanceled)
            return false;

        sale.IsCanceled = true;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
