using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Interfaces;

/// <summary>
/// Repository interface for managing sales.
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Creates a new sale.
    /// </summary>
    /// <param name="sale">The sale to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task CreateAsync(Sale sale, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a sale by its number.
    /// </summary>
    /// <param name="number">The sale number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sale if found; otherwise, null.</returns>
    Task<Sale> GetByNumberAsync(string number, CancellationToken cancellationToken);

    /// <summary>
    /// Lists sales.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of sales.</returns>
    Task<IEnumerable<Sale>> ListAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing sale.
    /// </summary>
    /// <param name="sale">The sale to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task UpdateAsync(Sale sale, CancellationToken cancellationToken);
}
