using LibreHardwareMonitor.Hardware;

namespace Worker
{
    public class GenerateEvents : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        private float? _value = 0;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Monitor, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Monitor(object? state)
        {
            Computer computer = new Computer
            {
                //IsCpuEnabled = true,
                //IsGpuEnabled = true,
                //IsMemoryEnabled = true,
                //IsMotherboardEnabled = true,
                //IsControllerEnabled = true,
                IsNetworkEnabled = true,
                //IsStorageEnabled = true,
                //IsBatteryEnabled = true,
                // IsPsuEnabled = true             
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
                    
                    Console.WriteLine("\tSensor: {0}, difference: {1}", sensor.Name, difference);

                    _value = sensor.Value;
                }
            }

            computer.Close();


            //foreach (IHardware hardware in computer.Hardware)
            //{
            //    Console.WriteLine("Hardware: {0}", hardware.Name);

            //    foreach (IHardware subhardware in hardware.SubHardware)
            //    {
            //        Console.WriteLine("\tSubhardware: {0}", subhardware.Name);

            //        foreach (ISensor sensor in subhardware.Sensors)
            //        {
            //            Console.WriteLine("\t\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
            //        }
            //    }

            //    foreach (ISensor sensor in hardware.Sensors)
            //    {
            //        Console.WriteLine("\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
            //    }
            //}
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
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
