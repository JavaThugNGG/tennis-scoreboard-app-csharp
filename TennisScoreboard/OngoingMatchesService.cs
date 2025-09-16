using System.Collections.Concurrent;
using TennisScoreboard;

public class OngoingMatchesService
{
    private readonly ConcurrentDictionary<Guid, MatchScoreModel> _currentMatches;
    private readonly List<CancellationTokenSource> _scheduledTasks;

    public OngoingMatchesService()
    {
        _currentMatches = new ConcurrentDictionary<Guid, MatchScoreModel>();
        _scheduledTasks = new List<CancellationTokenSource>();
    }

    public IReadOnlyDictionary<Guid, MatchScoreModel> CurrentMatches => _currentMatches;

    public Guid AddMatch(MatchScoreModel match)
    {
        var id = Guid.NewGuid();
        _currentMatches[id] = match;
        return id;
    }

    public void RemoveMatchWithDelay(Guid id, int delaySeconds)
    {
        var cts = new CancellationTokenSource();
        _scheduledTasks.Add(cts);

        Task.Delay(TimeSpan.FromSeconds(delaySeconds), cts.Token)
            .ContinueWith(t =>
            {
                if (!t.IsCanceled)
                    _currentMatches.TryRemove(id, out _);
            }, TaskScheduler.Default);
    }

    public void ShutdownScheduler()
    {
        foreach (var cts in _scheduledTasks)
        {
            cts.Cancel();
        }
        _scheduledTasks.Clear();
    }
}