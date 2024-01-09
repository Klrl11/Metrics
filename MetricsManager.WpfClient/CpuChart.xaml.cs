using LiveCharts;
using LiveCharts.Wpf;
using MetricsManagerNamespace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MetricsManager.WpfClient
{
    public partial class CpuChart : UserControl, INotifyPropertyChanged
    {
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private MetricsManagerClient _metricsManagerClient;

        private SeriesCollection _columnSeriesValues;
        private double _cpuPersent;

        public double CpuPersent
        {
            get { 
                return _cpuPersent;
            } 
            set { 
                _cpuPersent = value;
                NotifyPropertyChanged();
            }    
        }

        public SeriesCollection ColumnSeriesValues
        {
            get
            {
                return _columnSeriesValues;
            }
            set
            {
                _columnSeriesValues = value;
                NotifyPropertyChanged();
            }
        }

        public CpuChart()
        {
            InitializeComponent();
            DataContext = this;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                _dispatcherTimer.Start();
            }   
        }

        private async void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            if (PropertyChanged != null)
            {
                _metricsManagerClient = new MetricsManagerClient("http://localhost:5130", new HttpClient());
            }

            var cpuMetrics =
                await _metricsManagerClient.GetCpuMetricsAsync(1, DateTime.Now.AddSeconds(-60), DateTime.Now);

            if (cpuMetrics != null && cpuMetrics.Count > 0)
            {
                CpuPersent = cpuMetrics.Sum(x => x.Value) / cpuMetrics.Count;


                ColumnSeriesValues = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = new ChartValues<int>(cpuMetrics.Select(x => x.Value).ToArray())
                    }
                };

            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
