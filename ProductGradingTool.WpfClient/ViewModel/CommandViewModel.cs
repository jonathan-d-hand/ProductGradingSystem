namespace ProductGradingTool.WpfClient.ViewModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ProductGradingTool.WpfClient.Services;

public partial class CommandViewModel : ObservableObject
{
    //-dependencies.
    private readonly DialogService _dialogService;

    //-ctor.
    public CommandViewModel(DialogService dialogService)
    {
        _dialogService = dialogService;
    }

    [RelayCommand]
    private void ManualInput()
    {
        string result = _dialogService.ShowKeypadInputDialog("Product ID");
        if (string.IsNullOrEmpty(result)) return;

        WeakReferenceMessenger.Default.Send(new ProductIdChangedMessage(result));
    }

    [RelayCommand]
    private void ChangeUser()
    {
        string result = _dialogService.ShowKeypadInputDialog("User ID");
        if (string.IsNullOrEmpty(result)) return;

        WeakReferenceMessenger.Default.Send(new UserIdChangedMessage(result));
    }

    [RelayCommand]
    private void MarkContaminated()
    {
        
    }

    [RelayCommand]
    private void Submit()
    {
        WeakReferenceMessenger.Default.Send(new SubmitScanMessage(true));
    }

    [RelayCommand]
    private void Reset()
    {

    }

    [RelayCommand]
    private void Suspend()
    {

    }
}
