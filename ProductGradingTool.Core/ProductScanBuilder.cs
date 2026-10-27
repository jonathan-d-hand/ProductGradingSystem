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

    public bool CaptureEnabled
    {
        get
        {
            return _activeProductScan != null && !string.IsNullOrEmpty(_activeProductScan.ProductId)
                && !_activeProductScan.SetInvalid;
        }
    }

    //-ctor.
    public ProductScanBuilder(IProductScanRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc/>
    public void SetProductId(string productId)
    {
        ProductScan latest = _repository.QuerySaved(productId)
            .OrderByDescending(scan => scan.TimeStamp)
            .FirstOrDefault();

        _activeProductScan = new ProductScan()
        {
            ProductId = productId,
            UserId = _userId
        };

        if (latest != null)
        {
            _activeProductScan.Weight = latest.Weight;
            _activeProductScan.Grade = latest.Grade;
            _activeProductScan.SetInvalid = latest.SetInvalid;
        }

    }

    /// <inheritdoc/>
    public void SetUserId(string userId)
    {
        _userId = userId;
    }

    /// <inheritdoc/>
    public void CaptureGrade(GradeScan grade)
    {
        //todo: check capture enabled. (unit testing requirement)
        _activeProductScan.Grade = grade;
    }

    /// <inheritdoc/>
    public void CaptureWeight(string weight)
    {
        //todo: check capture enabled. (unit testing requirement)
        _activeProductScan.Weight = weight;
    }

    /// <inheritdoc/>
    public void MarkInvalid()
    {
        //todo: check capture enabled. (unit testing requirement)
        _activeProductScan.SetInvalid = true;
        _repository.Save(Result);
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
        Clear();
    }
}