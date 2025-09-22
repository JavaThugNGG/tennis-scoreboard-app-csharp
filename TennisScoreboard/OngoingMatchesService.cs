using System.Collections.Concurrent;
using TennisScoreboard;

public class OngoingMatchesService
{
    private readonly ConcurrentDictionary<Guid, MatchScoreModel> _currentMatches = new();
    private readonly ConcurrentDictionary<Guid, bool> _persistedMatches = new();
    private readonly MatchScheduler _scheduler;
    private readonly ILogger<OngoingMatchesService> _logger;

    public IReadOnlyDictionary<Guid, MatchScoreModel> CurrentMatches => _currentMatches;
    public IReadOnlyDictionary<Guid, bool> PersistedMatches => _persistedMatches;

    public OngoingMatchesService(MatchScheduler scheduler, ILogger<OngoingMatchesService> logger)
    {
        _scheduler = scheduler;
        _logger = logger;
    }

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
            _logger.LogInformation("Match {MatchId} removed successfully after {DelaySeconds}s", id, delaySeconds);
        }, delaySeconds);
    }

    public void ShutdownScheduler()
    {
        _scheduler.CancelAll();
    }

    public void MarkMatchAsPersisted(Guid matchId)
    {
        _persistedMatches[matchId] = true;
        _logger.LogInformation("Match {MatchId} marked as persisted", matchId);
    }
}