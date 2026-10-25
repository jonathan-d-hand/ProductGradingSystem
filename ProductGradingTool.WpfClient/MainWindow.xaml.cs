using ProductGradingTool.WpfClient.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProductGradingTool.WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            MainViewModel vm = DataContext as MainViewModel;
            KeypadInputWindow dialog = new KeypadInputWindow() { Owner = this };

            dialog.InputGroupBox.Header = "User ID";

            IsEnabled = false;
            if (dialog.ShowDialog() == true)
            {
                string uid = dialog.Value;
                IsEnabled = true;
                vm.StartWithUserId(uid);
            }
            else
            {
                Close();
            }
        }
    }
}