namespace ProductGradingTool.WpfClient.ViewModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ProductGradingTool.WpfClient.Services;
using ProductGradingTool.Model;
using CommunityToolkit.Mvvm.Input;

public partial class GradeCaptureViewModel : ObservableObject
{
    //-dependencies.
    private readonly DialogService _dialogService;

    public GradeCaptureViewModel(DialogService dialogService)
    {
        _dialogService = dialogService;
    }

    [ObservableProperty]
    public partial string FatValue { get; set; } = string.Empty;
    
    [ObservableProperty]
    public partial string LeanValue { get; set; } = string.Empty;
    
    [ObservableProperty]
    public partial string Grade { get; set; } = string.Empty;

    [RelayCommand]
    private void Input()
    {
        var result = new GradeScan();
        string dialogResult;
        //-todo: replace with 3-field keypad prompt.
        //~for now, 3 dialogs will be prompted asking for each value.
        //~all 3 values are required so any cancel button press aborts
        dialogResult = _dialogService.ShowKeypadInputDialog("FatValue");
        if (string.IsNullOrEmpty(dialogResult)) return;

        result.FatValue = dialogResult;

        dialogResult = _dialogService.ShowKeypadInputDialog("LeanValue");
        if (string.IsNullOrEmpty(dialogResult)) return;

        result.LeanValue = dialogResult;

        dialogResult = _dialogService.ShowKeypadInputDialog("LeanValue");
        if (string.IsNullOrEmpty(dialogResult)) return;

        result.Grade = dialogResult;

        //~no fields should be filled in until this point.
        FatValue = result.FatValue;
        LeanValue = result.LeanValue;
        Grade = result.Grade;

        WeakReferenceMessenger.Default.Send(new GradeChangedMessage(result));
    }

    [RelayCommand]
    private void Reset()
    {
        //todo: reset fields back to their original values.
    }
}