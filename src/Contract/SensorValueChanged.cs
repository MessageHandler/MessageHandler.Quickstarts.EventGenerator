namespace Contract
{
    public class SensorValueChanged
    {
        public float PreviousValue { get; set; }
        public float Value { get; set; }
        public string Unit { get; set; }
        public string DeviceId { get; set; }
        public string SensorId { get; set; }
    }
}
