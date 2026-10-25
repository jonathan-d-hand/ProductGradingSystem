using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProductGradingTool.WpfClient
{
    /// <summary>
    /// Interaction logic for KeypadInputWindow.xaml
    /// </summary>
    public partial class KeypadInputWindow : Window
    {
        public string Value => InputTextBox.Text;

        public KeypadInputWindow()
        {
            InitializeComponent();
        }

        private void OnNumericInput(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
                InputTextBox.AppendText(button.Content.ToString());
        }

        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
