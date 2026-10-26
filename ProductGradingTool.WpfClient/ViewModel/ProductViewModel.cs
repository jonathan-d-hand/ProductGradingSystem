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
        SAVED,    //-Green
        HISTORY,  //-Grey
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
        WeakReferenceMessenger.Default.Register<ResetTerminalMessage>(this, OnResetTerminal);
        WeakReferenceMessenger.Default.Register<SubmitScanMessage>(this, OnSubmitScan);
    }

    private void OnWeightChanged(object recipient, WeightChangedMessage message)
    {
        Weight = message.Value;
    }

    private void OnProductIdChanged(object recipient, ProductIdChangedMessage message)
    {
        ResetFields();
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

    private void OnResetTerminal(object recipient, ResetTerminalMessage message)
    {
        CurrentState = ProductViewState.NO_DATA;
        ResetFields();
    }

    private void OnSubmitScan(object recipient, SubmitScanMessage message)
    {
        CurrentState = ProductViewState.SAVED;
    }

    private void ResetFields()
    {
        ProductId = string.Empty;
        Weight = string.Empty;
        FatValue = string.Empty;
        LeanValue = string.Empty;
        Grade = string.Empty;
    }

}