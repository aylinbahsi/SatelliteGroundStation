using System;
using System.Collections.Generic;
using System.Text;

namespace SatelliteGroundStation.Models
{
    public class TelemetryData
    {
        public string Time { get; set; }
        public double Battery { get; set; }
        public double Temperature { get; set; }
        public double Altitude { get; set; }
        public int Signal { get; set; }
    }
}
