using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTemp.Model;

namespace WpfTemp.ViewModel
{
    public partial class ChartViewModel : ViewModelBase
    {
        private string[] _Labels;
        private double _GaugeValue;
        public ChartValues<double> _SeriesValues;

        public ChartValues<double> SeriesValues
        {
            get => _SeriesValues;
            set => SetProperty(ref _SeriesValues, value);
        }

        public string[] Labels
        {
            get => _Labels;
            set => SetProperty(ref _Labels, value);
        }

        public double GaugeValue
        {
            get => _GaugeValue;
            set => SetProperty(ref _GaugeValue, value);
        }
    }
}
