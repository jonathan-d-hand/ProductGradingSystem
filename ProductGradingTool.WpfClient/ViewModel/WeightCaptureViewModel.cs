namespace ProductGradingTool.WpfClient.ViewModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ProductGradingTool.WpfClient.Services;
using static ProductGradingTool.WpfClient.ViewModel.ProductViewModel;

public partial class WeightCaptureViewModel : ObservableObject
{
    private readonly DialogService _dialogService;

    public WeightCaptureViewModel(DialogService dialogService)
    {
        _dialogService = dialogService;

        WeakReferenceMessenger.Default.Register<ResetTerminalMessage>(this, OnResetTerminal);
        WeakReferenceMessenger.Default.Register<ProductIdChangedMessage>(this, OnProductIdChanged);
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Weight))]
    public partial string LiveWeight { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Weight))]
    public partial string CapturedWeight { get; set; } = null;

    public string Weight
    {
        get => !string.IsNullOrEmpty(CapturedWeight) ? CapturedWeight : LiveWeight;
    }

    [RelayCommand(CanExecute = nameof(CanCapture))]
    private void Capture()
    {
        //todo: Support capturing of live reading.
    }

    //todo: Support capturing of live reading.
    private bool CanCapture() => false;

    [RelayCommand]
    private void Input()
    {
        string result = _dialogService.ShowKeypadInputDialog("Weight");
        if (string.IsNullOrEmpty(result)) return;

        CapturedWeight = result;

        WeakReferenceMessenger.Default.Send(new WeightChangedMessage(CapturedWeight));
    }

    [RelayCommand]
    private void Reset()
    {
        CapturedWeight = null;
        WeakReferenceMessenger.Default.Send(new WeightChangedMessage(null));
    }

    private void OnResetTerminal(object recipient, ResetTerminalMessage message)
    {
        CapturedWeight = null;
    }

    private void OnProductIdChanged(object recipient, ProductIdChangedMessage message)
    {
        CapturedWeight = null;
    }

}
