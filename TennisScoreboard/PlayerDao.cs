namespace TennisScoreboard
{
    public class PlayerDao
    {
        public PlayerEntity GetByName(AppDbContext context, string playerName)
        {
            return context.Players.FirstOrDefault(p => p.Name == playerName);
        }

        public void Insert(AppDbContext context, PlayerEntity player)
        {
            context.Players.Add(player);
        }
    }
}
