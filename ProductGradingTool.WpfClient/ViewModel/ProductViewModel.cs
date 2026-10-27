namespace ProductGradingTool.WpfClient.ViewModel;

using CommunityToolkit.Mvvm.ComponentModel;
using ProductGradingTool.Model;

public enum ProductViewState
{
    NO_DATA,  //-Normal
    NEW_SCAN, //-Blue
    SAVED,    //-Green
    HISTORY,  //-Grey
    INVALID   //-Red
}

/// <summary>
/// View model interface for product information.
/// </summary>
public partial class ProductViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEditable))]
    public partial ProductViewState CurrentState { get; set; } = ProductViewState.NO_DATA;

    [ObservableProperty]
    public partial string ProductId { get; set; }

    [ObservableProperty]
    public partial string Weight { get; set; }

    [ObservableProperty]
    public partial string LeanValue { get; set; }
    
    [ObservableProperty]
    public partial string FatValue { get; set; }

    [ObservableProperty]
    public partial string Grade { get; set; }

    [ObservableProperty]
    public partial bool IsWeightCaptured { get; set; } = false;

    [ObservableProperty]
    public partial bool IsGradeCaptured { get; set; } = false;

    public bool IsEditable { get => CurrentState == ProductViewState.NEW_SCAN; }

    public void LoadFields(ProductScan productScan)
    {
        if (productScan.SetInvalid)
            CurrentState = ProductViewState.INVALID;
        else
            CurrentState = ProductViewState.NEW_SCAN;
        ProductId = productScan.ProductId;
        Weight = productScan.Weight;
        FatValue = productScan.Grade?.FatValue;
        LeanValue = productScan.Grade?.LeanValue;
        Grade = productScan.Grade?.Grade;
    }

    public void ResetFields()
    {
        CurrentState = ProductViewState.NO_DATA;
        ProductId = string.Empty;
        Weight = string.Empty;
        FatValue = string.Empty;
        LeanValue = string.Empty;
        Grade = string.Empty;
    }

}