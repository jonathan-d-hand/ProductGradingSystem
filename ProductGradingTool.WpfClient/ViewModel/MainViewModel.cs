namespace ProductGradingTool.WpfClient.ViewModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ProductGradingTool.Model;
using ProductGradingTool.WpfClient.Services;
using System.Collections.ObjectModel;

/// <summary>
/// This is the main view model intended for the MainWindow's context binding.
/// </summary>
public partial class MainViewModel : ObservableObject
{
    //-dependencies.
    private readonly DialogService _dialogService;
    private readonly IProductScanBuilder _scanBuilder;
    private readonly IProductHistory _history;
    
    //-view models.
    public CommandViewModel Command { get; set; }
    public ProductViewModel Product { get; private set; }
    public WeightCaptureViewModel WeightCapture { get; private set; }
    //todo: Implement GradeCaptureViewModel.

    [ObservableProperty] //todo: update data, don't replace the collection.
    private ObservableCollection<ProductHistoryItem> _productHistory;

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

    //-ctor.
    public MainViewModel(
        CommandViewModel command,
        ProductViewModel product,
        WeightCaptureViewModel weightCapture,
        DialogService dialogService,
        IProductScanBuilder scanBuilder,
        IProductHistory history)
    {
        Command = command;
        Product = product;
        WeightCapture = weightCapture;
        _dialogService = dialogService;
        _scanBuilder = scanBuilder;
        _history = history;

        WeakReferenceMessenger.Default.Register<UserIdChangedMessage>(this, OnUserIdChanged);
        WeakReferenceMessenger.Default.Register<ProductIdChangedMessage>(this, OnProductIdChanged);
        WeakReferenceMessenger.Default.Register<GradeChangedMessage>(this, OnGradeChanged);
        WeakReferenceMessenger.Default.Register<WeightChangedMessage>(this, OnWeightChanged);
        WeakReferenceMessenger.Default.Register<SubmitScanMessage>(this, OnSubmitScan);
        WeakReferenceMessenger.Default.Register<ResetTerminalMessage>(this, OnResetTerminal);
        WeakReferenceMessenger.Default.Register<MarkContaminatedMessage>(this, OnMarkContaminated);

        RefreshProductHistory();
    }

    private void RefreshProductHistory()
    {
        IEnumerable<ProductHistoryItem> historyItems = _history.LoadProductHistory();

        //todo: update data, don't replace the collection.
        ProductHistory = new ObservableCollection<ProductHistoryItem>(historyItems);
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

        Product.LoadFields(_scanBuilder.Result);

        Command.NotifyCanExecuteAll();
    }

    private void OnWeightChanged(object recipient, WeightChangedMessage message)
    {
        Product.Weight = message.Value;
        _scanBuilder.CaptureWeight(message.Value);
    }

    private void OnGradeChanged(object recipient, GradeChangedMessage message)
    {
        GradeScan grade = message.Value;
        Product.FatValue = grade.FatValue;
        Product.LeanValue = grade.LeanValue;
        Product.Grade = grade.Grade;
    }

    private void OnResetTerminal(object recipient, ResetTerminalMessage message)
    {
        Product.ResetFields();
        Command.NotifyCanExecuteAll();
    }

    private void OnSubmitScan(object recipient, SubmitScanMessage message)
    {
        _scanBuilder.Submit();
        Product.CurrentState = ProductViewState.SAVED;
        RefreshProductHistory();
        Command.NotifyCanExecuteAll();
    }

    private void OnMarkContaminated(object recipient, MarkContaminatedMessage message)
    {
        _scanBuilder.MarkInvalid();
        Product.CurrentState = ProductViewState.INVALID;
        RefreshProductHistory();
        Command.NotifyCanExecuteAll();
    }

    public void StartWithUserId(string userId)
    {
        UserId = userId;
        _scanBuilder.SetUserId(UserId);
    }


}