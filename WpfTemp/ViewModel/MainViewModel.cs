using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTemp.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfTemp.Model;

namespace WpfTemp.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        public ObservableCollection<ViewContent> ToggleItems { get; }

        [ObservableProperty]
        private ViewContent? _SelectedItem;

        [ObservableProperty]
        private string? _MainTitle = "WPF Template Application";

        [ObservableProperty]
        private string? _UserName;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MoveNextCommand))]
        [NotifyCanExecuteChangedFor(nameof(MovePrevCommand))]
        private int _selectedIndex;

        public MainViewModel()
        {
            ToggleItems = [
                            new ViewContent("HOME", typeof(Home)), 
                            new ViewContent("DEVICE", typeof(Devices)),
                            new ViewContent("CHART", typeof(Chart)),
                            new ViewContent("ECMAIN", typeof(ECMain))
                        ];
            SelectedItem = ToggleItems.First();
        }

        [RelayCommand(CanExecute = nameof(CanMovePrevious))]
        private void OnMovePrev()
        {
            SelectedIndex--;
        }

        private bool CanMovePrevious() => SelectedIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMoveNext))]
        private void OnMoveNext()
        {
            SelectedIndex++;
        }

        private bool CanMoveNext() => SelectedIndex < ToggleItems.Count - 1;

        [RelayCommand]
        private void OnHome()
        {
            SelectedIndex = 0;
        }

        [RelayCommand]
        private void OnPower()
        {
            Environment.Exit(0);
        }

        [RelayCommand]
        private void OnResize()
        {
            App.Current.MainWindow!.WindowState = App.Current.MainWindow.WindowState == System.Windows.WindowState.Maximized ? System.Windows.WindowState.Normal : System.Windows.WindowState.Maximized;
        }
    }
}
