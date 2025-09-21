namespace TennisScoreboard
{
    public class MatchPreparingService
    {
        private readonly PlayerService _playerService;

        public MatchPreparingService(PlayerService playerService)
        {
            _playerService = playerService;
        }

        public MatchScoreModel PersistPlayers(string firstPlayerName, string secondPlayerName)
        {
            int firstPlayerId = InsertPlayer(firstPlayerName);
            int secondPlayerId = InsertPlayer(secondPlayerName);
            return new MatchScoreModel(firstPlayerId, secondPlayerId);
        }

        private int InsertPlayer(string playerName)
        {
            try
            {
                return _playerService.Insert(playerName).Id;
            }
            catch (PlayerAlreadyExistsException)
            {
                return _playerService.GetByName(playerName).Id;
            }
        }
    }
}
