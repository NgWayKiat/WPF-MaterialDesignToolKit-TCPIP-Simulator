using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfTemp.Model;

namespace WpfTemp.ViewModel
{
    public partial class DeviceViewModel : ViewModelBase
    {
        private string? _CH0_VAL;
        private string? _CH1_VAL;
        private string? _CH2_VAL;
        private string? _CH3_VAL;   
        private string? _CH4_VAL;
        private string? _CH5_VAL;
        private string? _CH6_VAL;
        private string? _CH7_VAL;

        public string CH0_VAL
        {
            get => _CH0_VAL;
            set => SetProperty(ref _CH0_VAL, value);
        }

        public string CH1_VAL
        {
            get => _CH1_VAL;
            set => SetProperty(ref _CH1_VAL, value);
        }

        public string CH2_VAL
        {
            get => _CH2_VAL;
            set => SetProperty(ref _CH2_VAL, value);
        }

        public string CH3_VAL
        {
            get => _CH3_VAL;
            set => SetProperty(ref _CH3_VAL, value);
        }

        public string CH4_VAL
        {
            get => _CH4_VAL;
            set => SetProperty(ref _CH4_VAL, value);
        }

        public string CH5_VAL
        {
            get => _CH5_VAL;
            set => SetProperty(ref _CH5_VAL, value);
        }

        public string CH6_VAL
        {
            get => _CH6_VAL;
            set => SetProperty(ref _CH6_VAL, value);
        }

        public string CH7_VAL
        {
            get => _CH7_VAL;
            set => SetProperty(ref _CH7_VAL, value);
        }
    }

}
