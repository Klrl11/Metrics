using LiveCharts.Wpf;
using LiveCharts;
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
    /// <summary>
    /// Interaction logic for RamChart.xaml
    /// </summary>
    public partial class RamChart : UserControl , INotifyPropertyChanged
    {
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private MetricsManagerClient _metricsManagerClient;

        private SeriesCollection _columnSeriesValues;
        private double _ramPersent;

        public double RamPersent
        {
            get
            {
                return _ramPersent;
            }
            set
            {
                _ramPersent = value;
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

        public RamChart()
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

            var ramMetrics =
                await _metricsManagerClient.GetRamMetricsAsync(1, DateTime.Now.AddSeconds(-60), DateTime.Now);

            if (ramMetrics != null && ramMetrics.Count > 0)
            {
                RamPersent = ramMetrics.Sum(x => x.Value) / ramMetrics.Count;


                ColumnSeriesValues = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = new ChartValues<int>(ramMetrics.Select(x => x.Value).ToArray())
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
