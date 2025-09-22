namespace TennisScoreboard
{
    public class MatchScheduler : IDisposable
    {
        private readonly List<CancellationTokenSource> _tasks = new();
        private readonly ILogger<MatchScheduler> _logger;

        public MatchScheduler(ILogger<MatchScheduler> logger)
        {
            _logger = logger;
        }

        public void ScheduleAction(Action action, int delaySeconds)
        {
            var cts = new CancellationTokenSource();
            _tasks.Add(cts);

            Task.Delay(TimeSpan.FromSeconds(delaySeconds), cts.Token)
                .ContinueWith(t =>
                {
                    if (!t.IsCanceled)
                    {
                        try
                        {
                            action();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Exception occurred while executing scheduled action");
                        }
                    }
                }, TaskScheduler.Default);
        }

        public void CancelAll()
        {
            foreach (var cts in _tasks)
            {
                cts.Cancel();
            }
            _tasks.Clear();
            _logger.LogInformation("Cancelled {Count} scheduled tasks", _tasks.Count);
        }

        public void Dispose()
        {
            CancelAll();
            _logger.LogInformation("Scheduler disposed");
        }
    }

}