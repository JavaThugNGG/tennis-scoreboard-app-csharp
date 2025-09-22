using System.Collections.Concurrent;
using TennisScoreboard;

public class OngoingMatchesService
{
    private readonly ConcurrentDictionary<Guid, MatchScoreModel> _currentMatches = new();
    private readonly ConcurrentDictionary<Guid, bool> _persistedMatches = new();
    private readonly MatchScheduler _scheduler = new();

    public IReadOnlyDictionary<Guid, MatchScoreModel> CurrentMatches => _currentMatches;
    public IReadOnlyDictionary<Guid, bool> PersistedMatches => _persistedMatches;

    public Guid AddMatch(MatchScoreModel match)
    {
        var id = Guid.NewGuid();
        _currentMatches[id] = match;
        return id;
    }

    public void RemoveMatchWithDelay(Guid id, int delaySeconds)
    {
        _scheduler.ScheduleAction(() =>
        {
            _currentMatches.TryRemove(id, out _);
            _persistedMatches.TryRemove(id, out _);
        }, delaySeconds);
    }

    public void ShutdownScheduler()
    {
        _scheduler.CancelAll();
    }

    public void MarkMatchAsPersisted(Guid matchId)
    {
        _persistedMatches[matchId] = true;
    }
}