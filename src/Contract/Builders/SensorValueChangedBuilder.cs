using Contract;
using System;
using System.Collections.Generic;

namespace Contract
{
    public class SensorValueChangedBuilder
    {
        private SensorValueChanged _event;

        public SensorValueChangedBuilder()
        {
            _event = new SensorValueChanged
            {
                PreviousValue = 0,
                Value = 0,
                Unit = "bytes",
                DeviceId = string.Empty,
                SensorId = string.Empty,
            };
        }

        public SensorValueChangedBuilder WellknownSensor(string bookingId)
        {
            if (_wellknownEvents.ContainsKey(bookingId))
            {
                _event = _wellknownEvents[bookingId]();
            }

            return this;
        }

        public SensorValueChanged Build()
        {
            return _event;
        }

        private readonly Dictionary<string, Func<SensorValueChanged>> _wellknownEvents = new()
        {
            {
                "D98204AA-5328-4078-8D66-354286514994",
                () =>
                {
                    return new SensorValueChanged()
                    {
                        PreviousValue = 0,
                        Value = 1.0035226F,
                        Unit = "bytes",
                        DeviceId = "LAPTOP-OKS157HK",
                        SensorId = "/nic/{D98204AA-5328-4078-8D66-354286514994}/data/3",
                    };
                }                
            }
        };
    }
}
