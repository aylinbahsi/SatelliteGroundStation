# 🛰️ UAV / Satellite Ground Control Station

A professional, real-time Ground Control Station (GCS) software developed using **C# .NET** and **WPF**. Designed to monitor unmanned aerial vehicles (UAVs) and satellite systems telemetry data seamlessly.

This project implements the **MVVM (Model-View-ViewModel)** architectural pattern and utilizes **UDP networking** to receive asynchronous telemetry packets without blocking the main UI thread.

## ✨ Features
* **Real-Time Data Parsing:** Listens to UDP Port 14550 to capture incoming JSON telemetry payloads.
* **MVVM Architecture:** Strict separation of UI (XAML) and backend logic (C#) via Data Binding and ICommand interfaces.
* **Modern UI/UX:** Responsive, dark-themed dashboard tailored for aerospace control rooms.
* **Telemetry Logger:** Live updating DataGrid table with `ObservableCollection` for historical flight data tracking.
* **Python Simulator:** Includes a built-in Python script (`uav_simulator.py`) to simulate flight dynamics and broadcast test network packets.

## 🛠️ Tech Stack
* **Frontend:** WPF (Windows Presentation Foundation), XAML
* **Backend:** C#, .NET
* **Networking:** UDP Sockets (`UdpClient`), Asynchronous Tasks (`Task.Run`)
* **Testing:** Python (Socket & JSON libraries)

## 🚀 How to Run
1. Clone this repository to your local machine.
2. Open the solution in **Visual Studio** and build the project.
3. Run the `SatelliteGroundStation` application and click the **START TELEMETRY** button to begin listening on port 14550.
4. Open a terminal and run the Python simulator to start broadcasting data:
   ```bash
   python uav_simulator.py
