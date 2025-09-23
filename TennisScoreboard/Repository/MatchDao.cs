using Microsoft.EntityFrameworkCore;
using TennisScoreboard.Entities;
using TennisScoreboard.Infrastructure;

namespace TennisScoreboard.Repository
{
    public class MatchDao
    {
        public IList<MatchEntity> GetPage(AppDbContext context, int matchesPerPage, int paginationStartIndex)
        {
            return context.Matches
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .Include(m => m.Winner)
                .OrderByDescending(m => m.Id)
                .Skip(paginationStartIndex)
                .Take(matchesPerPage)
                .ToList();
        }

        public List<MatchEntity> GetPageWithPlayerFilter(AppDbContext context, PlayerEntity player, int matchesPerPage, int paginationStartIndex)
        {
            return context.Matches
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .Include(m => m.Winner)
                .Where(m => m.Player1 == player || m.Player2 == player)
                .OrderByDescending(m => m.Id)
                .Skip(paginationStartIndex)
                .Take(matchesPerPage)
                .ToList();
        }

        public long Count(AppDbContext context)
        {
            return context.Matches.LongCount();
        }

        public long CountWithPlayerFilter(AppDbContext context, PlayerEntity player)
        {
            return context.Matches
                .Where(m => m.Player1 == player || m.Player2 == player)
                .LongCount();
        }
    }
}
