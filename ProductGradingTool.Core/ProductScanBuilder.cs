namespace ProductGradingTool;

using Model;
using Data;

/// <inheritdoc/>
/// <remarks>
/// This is the default implementation of the IProductScanBuilder interface.
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
        _activeProductScan = new ProductScan()
        {
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