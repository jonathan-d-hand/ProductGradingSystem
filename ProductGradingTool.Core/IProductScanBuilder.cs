namespace ProductGradingTool;

using Model;

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
    /// This flag determines if data can be captured.
    /// </summary>
    bool CaptureEnabled { get; }

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