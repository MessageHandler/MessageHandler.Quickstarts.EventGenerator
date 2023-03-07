namespace Worker
{
    public class RunEventGenerator : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        private IGenerateEvents eventGenerator;

        public RunEventGenerator(IGenerateEvents eventGenerator)
        {
            this.eventGenerator = eventGenerator;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(state => eventGenerator.Generate(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }      

        public void Dispose()
        {
            _timer?.Dispose();
        }        
    }    
}
