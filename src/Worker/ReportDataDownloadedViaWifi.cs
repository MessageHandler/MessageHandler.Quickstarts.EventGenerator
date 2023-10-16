using Contract;
using LibreHardwareMonitor.Hardware;
using MessageHandler.Runtime.StreamProcessing;

namespace Worker
{
    public class ReportDataDownloadedViaWifi : IGenerateEvents
    {
        private float? _value = 0;
        private IDispatchMessages dispatcher;

        public ReportDataDownloadedViaWifi(IDispatchMessages dispatcher) 
        {
            this.dispatcher = dispatcher;
        }

        public async Task Generate()
        {
            Computer computer = new Computer
            {
                IsNetworkEnabled = true
            };

            computer.Open();
            computer.Accept(new HardwareVisitor());

            var wifi = computer.Hardware.FirstOrDefault(h => h.Name == "Wi-Fi");
            if (wifi != null)
            {
                var sensor = wifi.Sensors.FirstOrDefault(s => s.Name == "Data Downloaded");
                if (sensor != null)
                {
                    var difference = sensor.Value - _value;

                    Console.WriteLine("\tSensor: {0}, Value: {1}", sensor.Name, sensor.Value);

                    if (_value != sensor.Value)
                    {
                        await this.dispatcher.Dispatch(new[] {
                           new SensorValueChanged()
                           {
                                PreviousValue = _value.HasValue ? _value.Value : 0,
                                Value = sensor.Value.HasValue ? sensor.Value.Value : 0,
                                Unit = "bytes",
                                DeviceId = System.Environment.MachineName,
                                SensorId = sensor.Identifier.ToString()
                           }
                       });
                    }

                    _value = sensor.Value;

                    
                }
            }

            computer.Close();
        }

        public class HardwareVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }
            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }
            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
        }
    }
}
