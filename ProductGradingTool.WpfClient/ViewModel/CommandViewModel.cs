namespace ProductGradingTool.WpfClient.ViewModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ProductGradingTool.WpfClient.Services;
using System.Windows.Navigation;

public partial class CommandViewModel : ObservableObject
{
    //-dependencies.
    private readonly DialogService _dialogService;
    private readonly IProductScanBuilder _scanBuilder;

    //-ctor.
    public CommandViewModel(DialogService dialogService, IProductScanBuilder scanBuilder)
    {
        _dialogService = dialogService;
        _scanBuilder = scanBuilder;
    }

    public void NotifyCanExecuteAll()
    {
        SubmitCommand.NotifyCanExecuteChanged();
        MarkContaminatedCommand.NotifyCanExecuteChanged();
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

    [RelayCommand(CanExecute = nameof(CanMarkContaminated))]
    private void MarkContaminated()
    {
        //todo: Implement warning dialog box for marking a product contaminated.
        WeakReferenceMessenger.Default.Send(new MarkContaminatedMessage(true));
    }

    //todo: Implement disqualifying process for product.
    private bool CanMarkContaminated()
    {
        return _scanBuilder.CaptureEnabled;
    }

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private void Submit()
    {
        WeakReferenceMessenger.Default.Send(new SubmitScanMessage(true));
    }

    private bool CanSubmit()
    {
        return _scanBuilder.CaptureEnabled;
    }

    [RelayCommand]
    private void Reset()
    {
        WeakReferenceMessenger.Default.Send(new ResetTerminalMessage(true));
    }

    [RelayCommand(CanExecute = nameof(CanSuspend))]
    private void Suspend()
    {
        //todo: Implement screen cleaning process.
    }

    //todo: Implement screen cleaning process.
    private bool CanSuspend() => false;
}
