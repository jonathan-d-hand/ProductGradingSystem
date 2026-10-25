namespace ProductGradingTool.WpfClient.ViewModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ProductGradingTool.WpfClient.Services;

public partial class WeightCaptureViewModel : ObservableObject
{
    private readonly DialogService _dialogService;

    public WeightCaptureViewModel(DialogService dialogService)
    {
        _dialogService = dialogService;
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

    [RelayCommand]
    private void Capture()
    {
        CapturedWeight = LiveWeight;

        //_scanBuilder.CaptureWeight(CapturedWeight.Value);
    }

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
    }
}
