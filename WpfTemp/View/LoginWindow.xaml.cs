using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfTemp.Model;
using WpfTemp.Views;
using static System.Net.Mime.MediaTypeNames;

namespace WpfTemp.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Login_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SignIn.IsEnabled = Login?.Text.TrimEnd() != "" && Password?.Password != "";
        }

        private void Password_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            SignIn.IsEnabled = Login?.Text.TrimEnd() != "" && Password?.Password != "";
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SignIn.IsEnabled)
                SignIn_OnClick(SignIn, new RoutedEventArgs());
        }

        private void SignIn_OnClick(object sender, RoutedEventArgs e)
        {
            Global.sUsrName = Login.Text;
            Global.sUsrPassword = Password.Password;

            IsEnabled = false;
            LoadingWindow loadingWindow = new LoadingWindow
            {
                Owner = this,
                Width = Width - 40,
                Height = Height - 40
            };
            loadingWindow.Show();

            Thread connectThread = new Thread(() =>
            {
                try {

                    Dispatcher.Invoke(() =>
                    {
                        loadingWindow.Hide();
                        DialogResult = true;
                    });
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        loadingWindow.Close();
                        IsEnabled = true;
                        ErrorMessage.Visibility = Visibility.Visible;
                        ErrorMessage.Text = ex.Message.ToString();
                    });
                    return;
                }
            });
            connectThread.Start();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
