namespace ProductGradingTool;

using Model;
using Data;

/// <summary>Tool for constructing a ProductScan.</summary>
public interface IProductScanBuilder
{
    ProductScan Result { get; }
    void SetProductId(string productId);
    void SetUserId(string userId);
    void CaptureWeight(string weight);
    void CaptureGrade(GradeScan grade);
    void Clear();
    void MarkInvalid();
    void Submit();
}

/// <inheritdoc/>
public class ProductScanBuilder : IProductScanBuilder
{
    //-dependencies.
    private readonly IProductScanRepository _repository;

    private ProductScan _activeProductScan = null;
    private string _userId = string.Empty;

    public ProductScan Result { get => _activeProductScan; }

    public ProductScanBuilder(IProductScanRepository repository)
    {
        _repository = repository;
    }

    public void SetProductId(string productId)
    {
        _activeProductScan = new ProductScan() {
            ProductId = productId,
            UserId = _userId
        };
    }

    public void SetUserId(string userId)
    {
        _userId = userId;
    }

    public void CaptureGrade(GradeScan grade)
    {
        _activeProductScan.Grade = grade;
    }

    public void CaptureWeight(string weight)
    {
        _activeProductScan.Weight = weight;
    }

    public void MarkInvalid()
    {
        _activeProductScan.SetInvalid = true;
    }

    public void Clear()
    {
        _activeProductScan = null;
    }

    public void Submit()
    {
        //todo: validation.
        _repository.Save(Result);
    }
}