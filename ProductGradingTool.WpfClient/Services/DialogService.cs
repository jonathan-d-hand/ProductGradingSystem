namespace ProductGradingTool.WpfClient.Services;

using System.Windows;

public class DialogService
{
    public string ShowKeypadInputDialog(string label)
    {
        var window = new KeypadInputWindow()
        {
            Owner = Application.Current.MainWindow
        };
        window.InputGroupBox.Header = label;

        if (window.ShowDialog() == true)
            return window.Value;
        else
            return string.Empty;
    }
}