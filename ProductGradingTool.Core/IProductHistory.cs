namespace ProductGradingTool;

using Model;

/// <summary>
/// Service that provides a history of product scans.
/// </summary>
public interface IProductHistory
{
    /// <summary>
    /// Retrieves a list of product scans.
    /// </summary>
    IEnumerable<ProductHistoryItem> LoadProductHistory();

    /// <summary>
    /// Retrieves the scan data for a specified product id.
    /// </summary>
    /// <returns>The latest scan data for the product.</returns>
    ProductScan LoadLatestScan(string productId);
}
