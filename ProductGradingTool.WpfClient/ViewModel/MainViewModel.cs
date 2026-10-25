namespace ProductGradingTool.WpfClient.ViewModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ProductGradingTool.WpfClient.Services;

/// <summary>
/// This is the main view model intended for the MainWindow's context binding.
/// </summary>
public partial class MainViewModel : ObservableObject
{
    private readonly DialogService _dialogService;
    private readonly IProductScanBuilder _scanBuilder;
    
    //-ViewModels
    public CommandViewModel Command { get; set; }
    public ProductViewModel Product { get; private set; }
    public WeightCaptureViewModel WeightCapture { get; private set; }

    public MainViewModel(
        CommandViewModel command,
        ProductViewModel product,
        WeightCaptureViewModel weightCapture,
        DialogService dialogService, 
        IProductScanBuilder scanBuilder)
    {
        Command = command;
        Product = product;
        WeightCapture = weightCapture;
        _dialogService = dialogService;
        _scanBuilder = scanBuilder;
        
        WeakReferenceMessenger.Default.Register<UserIdChangedMessage>(this, OnUserIdChanged);
        WeakReferenceMessenger.Default.Register<ProductIdChangedMessage>(this, OnProductIdChanged);
        WeakReferenceMessenger.Default.Register<WeightChangedMessage>(this, OnWeightChanged);
        WeakReferenceMessenger.Default.Register<SubmitScanMessage>(this, OnSubmitScan);
    }

    //todo: Implement GradeCaptureViewModel.

    /// <summary>
    /// Signed-in User ID logged by the user.
    /// </summary>
    [ObservableProperty]
    public partial string UserId { get; set; }

    /// <summary>
    /// The identification label on the terminal.
    /// </summary>
    [ObservableProperty]
    public partial string TerminalId { get; set; }

    /// <summary>
    /// The grouping identifier for consolidating within predefined batches.
    /// </summary>
    [ObservableProperty]
    public partial string BatchId { get; set; } = "NA";

    public void StartWithUserId(string userId)
    {
        UserId = userId;
        _scanBuilder.SetUserId(UserId);
    }

    private void OnUserIdChanged(object recipient, UserIdChangedMessage message)
    {
        string userId = message.Value;
        _scanBuilder.SetUserId(UserId);
        UserId = userId;
    }

    private void OnProductIdChanged(object recipient, ProductIdChangedMessage message)
    {
        _scanBuilder.SetProductId(message.Value);
    }

    private void OnWeightChanged(object recipient, WeightChangedMessage message)
    {
        _scanBuilder.CaptureWeight(message.Value);
    }

    private void OnSubmitScan(object recipient, SubmitScanMessage message)
    {
        _scanBuilder.Submit();
    }

}