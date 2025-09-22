namespace TennisScoreboard
{
    public class MatchScheduler : IDisposable
    {
        private readonly List<CancellationTokenSource> _tasks = new();

        public void ScheduleAction(Action action, int delaySeconds)
        {
            var cts = new CancellationTokenSource();
            _tasks.Add(cts);

            Task.Delay(TimeSpan.FromSeconds(delaySeconds), cts.Token)
                .ContinueWith(t =>
                {
                    if (!t.IsCanceled)
                    {
                        action();
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
        }

        public void Dispose()
        {
            CancelAll();
        }
    }

}