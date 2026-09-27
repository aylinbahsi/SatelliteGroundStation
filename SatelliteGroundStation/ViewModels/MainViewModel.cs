using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using SatelliteGroundStation.Models;
using SatelliteGroundStation.Services; // Yeni yazdığımız servisi dahil ettik

namespace SatelliteGroundStation.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private TelemetryData _telemetry;
        private UdpTelemetryService _udpService; // Simülatör yerine ağ servisi geldi

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ObservableCollection<TelemetryData> TelemetryHistory { get; set; }

        public TelemetryData Telemetry
        {
            get { return _telemetry; }
            set
            {
                _telemetry = value;
                OnPropertyChanged("Telemetry");
            }
        }

        public MainViewModel()
        {
            TelemetryHistory = new ObservableCollection<TelemetryData>();

            // Uygulama ilk açıldığında sıfır değerleri göster (Veri bekleniyor)
            Telemetry = new TelemetryData
            {
                Battery = 0.0,
                Temperature = 0.0,
                Altitude = 0.0,
                Signal = 0
            };

            // UDP Ağ Servisini kuruyoruz
            _udpService = new UdpTelemetryService();
            _udpService.TelemetryReceived += OnTelemetryReceived; // Dışarıdan veri gelince bu metodu tetikle

            // XAML'daki butonları UDP servisini başlatma/durdurma işlemine bağladık
            StartCommand = new RelayCommand(() => _udpService.StartListening());
            StopCommand = new RelayCommand(() => _udpService.StopListening());
        }

        // Ağ portundan (14550) her yeni veri paketi ulaştığında burası çalışacak
        private void OnTelemetryReceived(object sender, TelemetryData e)
        {
            // UDP servisi arka planda çalıştığı için, arayüzü (UI) güncellerken ana thread'e geçmemiz gerekiyor.
            // WPF'te bunu Dispatcher.Invoke ile yaparız. (Mülakatlarda harika bir detaydır)
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                Telemetry = e; // Ekrandaki kartları güncelle
                TelemetryHistory.Insert(0, Telemetry); // Tablonun en üstüne ekle
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}