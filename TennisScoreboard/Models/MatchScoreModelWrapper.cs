namespace TennisScoreboard.Models
{
    public class MatchScoreModelWrapper
    {
        private readonly MatchScoreModel _match;
        private readonly PlayerSide _player;

        public MatchScoreModelWrapper(MatchScoreModel match, PlayerSide player)
        {
            _match = match;
            _player = player;
        }

        public int Points
        {
            get
            {
                if (_player == PlayerSide.First)
                    return _match.FirstPlayerPoints;
                else
                    return _match.SecondPlayerPoints;
            }
            set
            {
                if (_player == PlayerSide.First)
                    _match.FirstPlayerPoints = value;
                else
                    _match.SecondPlayerPoints = value;
            }
        }

        public int Games
        {
            get
            {
                if (_player == PlayerSide.First)
                    return _match.FirstPlayerGames;
                else
                    return _match.SecondPlayerGames;
            }
            set
            {
                if (_player == PlayerSide.First)
                    _match.FirstPlayerGames = value;
                else
                    _match.SecondPlayerGames = value;
            }
        }

        public int Sets
        {
            get
            {
                if (_player == PlayerSide.First)
                    return _match.FirstPlayerSets;
                else
                    return _match.SecondPlayerSets;
            }
            set
            {
                if (_player == PlayerSide.First)
                    _match.FirstPlayerSets = value;
                else
                    _match.SecondPlayerSets = value;
            }
        }

        public bool HasAdvantage
        {
            get
            {
                if (_player == PlayerSide.First)
                    return _match.FirstPlayerAdvantage;
                else
                    return _match.SecondPlayerAdvantage;
            }
            set
            {
                if (_player == PlayerSide.First)
                    _match.FirstPlayerAdvantage = value;
                else
                    _match.SecondPlayerAdvantage = value;
            }
        }
    }
}
