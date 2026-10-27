namespace ProductGradingTool;

using Model;
using Data;

/// <inheritdoc/>
/// <remarks>
/// This is the default implementation of the IProductHistory interface.
/// </remarks>
public class ProductHistory : IProductHistory
{
    //-dependencies.
    private readonly IProductScanRepository _repository;

    //-ctor.
    public ProductHistory(IProductScanRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc/>
    public ProductScan LoadLatestScan(string productId)
    {
        IEnumerable<ProductScan> productScans = _repository.QuerySaved(productId);

        return productScans.OrderByDescending(productScan => productScan.TimeStamp)
            .FirstOrDefault();
    }

    /// <inheritdoc/>
    public IEnumerable<ProductHistoryItem> LoadProductHistory()
    {
        IEnumerable<ProductScan> productScans = _repository.QuerySaved();

        return productScans.GroupBy(productScan => productScan.ProductId)
            .Select(grouping => {
                ProductScan latestScan = grouping.OrderByDescending(productScan => productScan.TimeStamp).First();
                return new ProductHistoryItem
                {
                    ProductId = grouping.Key,
                    TimeStamp = latestScan.TimeStamp,
                    IsInvalid = grouping.Any(productScan => productScan.SetInvalid)
                };
            })
            .OrderByDescending(historyItem => historyItem.TimeStamp);
    }
}