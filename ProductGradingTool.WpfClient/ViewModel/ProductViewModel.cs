namespace ProductGradingTool.WpfClient.ViewModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ProductGradingTool.Model;

/// <summary>
/// View model interface for product information.
/// </summary>
public partial class ProductViewModel : ObservableObject
{
    public enum ProductViewState
    {
        NO_DATA,  //-Normal
        NEW_SCAN, //-Blue
        HISTORY,  //-Green
        INVALID   //-Red
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEditable))]
    public partial ProductViewState CurrentState { get; set; } = ProductViewState.NO_DATA;

    /// <summary>
    /// The current working product ID.
    /// </summary>
    /// <remarks>
    /// This is normally scanned in by a device or manual input.
    /// </remarks>
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

    public ProductViewModel()
    {
        WeakReferenceMessenger.Default.Register<WeightChangedMessage>(this, OnWeightChanged);
        WeakReferenceMessenger.Default.Register<ProductIdChangedMessage>(this, OnProductIdChanged);
        WeakReferenceMessenger.Default.Register<GradeChangedMessage>(this, OnGradeChanged);
    }

    private void OnWeightChanged(object recipient, WeightChangedMessage message)
    {
        Weight = message.Value;
    }

    private void OnProductIdChanged(object recipient, ProductIdChangedMessage message)
    {
        ProductId = message.Value;
        CurrentState = ProductViewState.NEW_SCAN;
    }

    private void OnGradeChanged(object recipient, GradeChangedMessage message)
    {
        GradeScan grade = message.Value;
        FatValue = grade.FatValue;
        LeanValue = grade.LeanValue;
        Grade = grade.Grade;
    }

}