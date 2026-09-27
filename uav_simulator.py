import socket
import json
import time
import random

UDP_IP = "127.0.0.1" # Kendi bilgisayarımız (Localhost)
UDP_PORT = 14550     # Yer istasyonunun dinlediği port

# UDP soketi oluşturuyoruz
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

battery = 100.0
altitude = 0.0

print(f"UAV Simülatörü Başladı! Hedef: {UDP_IP}:{UDP_PORT}")
print("Telemetri verileri gönderiliyor... (Durdurmak için CTRL+C)")

try:
    while True:
        # Rastgele uçuş dinamikleri üret
        battery = max(0.0, battery - random.uniform(0.05, 0.2))
        temp = random.uniform(20.0, 30.0)
        altitude += random.uniform(2.0, 5.0)
        signal = random.randint(-90, -40)

        # C# tarafındaki TelemetryData modeliyle birebir aynı JSON yapısı
        telemetry_packet = {
            "Battery": battery,
            "Temperature": temp,
            "Altitude": altitude,
            "Signal": signal
        }

        # Veriyi JSON formatına çevir ve byte olarak ağa yolla
        json_message = json.dumps(telemetry_packet)
        sock.sendto(json_message.encode('utf-8'), (UDP_IP, UDP_PORT))

        print(f"Gönderilen: {json_message}")
        time.sleep(1) # Saniyede 1 paket (1 Hz)

except KeyboardInterrupt:
    print("\nUçuş sonlandırıldı.")