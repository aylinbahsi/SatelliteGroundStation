using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SatelliteGroundStation.Models;

namespace SatelliteGroundStation.Services
{
    public class UdpTelemetryService
    {
        private UdpClient _udpClient;
        private bool _isListening;
        private readonly int _port = 14550; // İHA/Uydu sistemlerinde sık kullanılan bir port

        // Dışarıdan veri geldiğinde ViewModel'i uyaracak olan Olay (Event)
        public event EventHandler<TelemetryData> TelemetryReceived;

        public void StartListening()
        {
            if (_isListening) return;

            // Belirtilen porttan gelen paketleri dinlemek için UdpClient başlatıyoruz
            _udpClient = new UdpClient(_port);
            _isListening = true;

            // Arayüzün donmaması için dinleme işlemini arka planda (Task) çalıştırıyoruz
            Task.Run(ReceiveLoop);
        }

        public void StopListening()
        {
            _isListening = false;
            _udpClient?.Close();
            _udpClient = null;
        }

        private async Task ReceiveLoop()
        {
            while (_isListening)
            {
                try
                {
                    // Ağa bir paket gelene kadar burada bekler
                    UdpReceiveResult result = await _udpClient.ReceiveAsync();

                    // Gelen byte dizisini string'e çeviriyoruz (JSON formatında gelecek)
                    string jsonString = Encoding.UTF8.GetString(result.Buffer);

                    // JSON string'ini TelemetryData nesnesine dönüştürüyoruz
                    TelemetryData data = JsonSerializer.Deserialize<TelemetryData>(jsonString);
                    data.Time = DateTime.Now.ToString("HH:mm:ss"); // Saati yer istasyonunda ekliyoruz

                    // Veri başarıyla çözüldüyse, arayüze (ViewModel'e) gönder
                    TelemetryReceived?.Invoke(this, data);
                }
                catch (Exception)
                {
                    // Bağlantı koparsa veya program kapanırsa çökmeyi engellemek için
                }
            }
        }
    }
}
