using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTemp.Model;

namespace WpfTemp.ViewModel
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty] 
        private OpenDrawers _OpenDrawers = new();

        [ObservableProperty]
        private TcpClientModel _TcpClientModel = new();

        [RelayCommand]
        public void Execute(string obj) {
            switch (obj)
            {
                case "OpenLeftDrawer": OpenDrawers.LeftDrawer = true; break;
                default: break;
            }
        }
    }
}
