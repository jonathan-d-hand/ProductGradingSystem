namespace ProductGradingTool;

using Model;
using Data;

/// <summary>
/// Main service that collects data for a ProductScan.
/// </summary>
public interface IProductScanBuilder
{
    /// <summary>
    /// This is the current result of the product scan.
    /// </summary>
    ProductScan Result { get; }

    /// <summary>
    /// Sets the product ID which will reset the scan process.
    /// </summary>
    void SetProductId(string productId);

    /// <summary>
    /// Sets the user ID.
    /// </summary>
    /// <remarks>
    /// This will not reset when the product ID changes.
    /// </remarks>
    void SetUserId(string userId);

    /// <summary>
    /// Assigns the weight data to product scan.
    /// </summary>
    void CaptureWeight(string weight);
    
    /// <summary>
    /// Assigns the grade scores to the product scan.
    /// </summary>
    void CaptureGrade(GradeScan grade);

    /// <summary>
    /// Clears the current product scan entirely.
    /// </summary>
    void Clear();

    /// <summary>
    /// Marks the product scan as invalid and submits it immediately.
    /// </summary>
    void MarkInvalid();

    /// <summary>
    /// Submits the product scan to the local repository.
    /// </summary>
    void Submit();
}

/// <inheritdoc/>
/// <remarks>
/// This is the default implementation.
/// </remarks>
public class ProductScanBuilder : IProductScanBuilder
{
    //-dependencies.
    private readonly IProductScanRepository _repository;

    //-members.
    private ProductScan _activeProductScan = null;
    private string _userId = string.Empty;

    /// <inheritdoc/>
    public ProductScan Result { get => _activeProductScan; }

    //-ctor.
    public ProductScanBuilder(IProductScanRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc/>
    public void SetProductId(string productId)
    {
        _activeProductScan = new ProductScan() {
            ProductId = productId,
            UserId = _userId
        };
    }

    /// <inheritdoc/>
    public void SetUserId(string userId)
    {
        _userId = userId;
    }

    /// <inheritdoc/>
    public void CaptureGrade(GradeScan grade)
    {
        _activeProductScan.Grade = grade;
    }

    /// <inheritdoc/>
    public void CaptureWeight(string weight)
    {
        _activeProductScan.Weight = weight;
    }

    /// <inheritdoc/>
    public void MarkInvalid()
    {
        _activeProductScan.SetInvalid = true;
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _activeProductScan = null;
    }

    /// <inheritdoc/>
    public void Submit()
    {
        _repository.Save(Result);
    }
}