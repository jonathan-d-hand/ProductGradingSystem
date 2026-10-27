namespace ProductGradingTool.Data;

using Model;
using System.Collections.Immutable;

/// <summary>Repository service for product scan submissions.</summary>
public interface IProductScanRepository
{
    IEnumerable<ProductScan> QuerySaved();
    IEnumerable<ProductScan> QuerySaved(string productId);
    void Save(ProductScan scan);
}

/// <summary>A sample in-memory implementation of the IProductScanRepository interface.</summary>
/// <remarks>Useful for quick scenario tests.</remarks>
public class InMemoryProductScanRepository : IProductScanRepository
{
    private readonly List<ProductScan> _saved = new List<ProductScan>();

    public IEnumerable<ProductScan> QuerySaved()
    {
        return _saved.ToImmutableArray();
    }

    public IEnumerable<ProductScan> QuerySaved(string productId)
    {
        return _saved.Where(x => x.ProductId == productId).ToImmutableArray();
    }

    public void Save(ProductScan scan)
    {
        _saved.Add(scan);
    }
}
