using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using WpfTemp.Model;
using WpfTemp.ViewModel;

namespace WpfTemp.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Snackbar Snackbar = new();
        private ClientSocket objClientSocket  = new ClientSocket();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var mainViewModel = new MainViewModel();
            DataContext = mainViewModel;
            mainViewModel.UserName = " [Welcome, " + Global.sUsrName.ToString().ToUpper() + "] ";
        }

        private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void OnSelectedItemChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MainScrollViewer.ScrollToHome();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen flag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;

            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            MenuToggleButton.IsChecked = false;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
