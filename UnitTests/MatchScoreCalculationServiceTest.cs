using TennisScoreboard;


namespace UnitTests
{
    public class MatchScoreCalculationServiceTest
    {
        private readonly int FirstPlayerId = 1;
        private readonly int SecondPlayerId = 2;

        private readonly MatchScoreCalculationService matchScoreCalculationService = new ();
        private MatchScoreModel match;

        public MatchScoreCalculationServiceTest()
        {
            match = new MatchScoreModel(FirstPlayerId, SecondPlayerId);
        }

        public static TheoryData<PlayerSide> AllSides => new()
        {
            PlayerSide.First,
            PlayerSide.Second
        };

        [Theory]
        [MemberData(nameof(AllSides))]
        public void ScoringPointsFromLoveShouldUpdatePointsToFifteen(PlayerSide scorerSide)
        {
            var scorer = new MatchScoreModelWrapper(match, scorerSide);

            matchScoreCalculationService.Scoring(match, scorerSide);

            Assert.Equal(TennisPoint.Fifteen, scorer.Points);
        }

        [Theory]
        [MemberData(nameof(AllSides))]
        public void ScoringPointsFromFifteenShouldUpdatePointsToThirty(PlayerSide scorerSide)
        {
            var scorer = new MatchScoreModelWrapper(match, scorerSide);

            scorer.Points = TennisPoint.Fifteen;
            matchScoreCalculationService.Scoring(match, scorerSide);

            Assert.Equal(TennisPoint.Thirty, scorer.Points);
        }

        [Theory]
        [MemberData(nameof(AllSides))]
        public void ScoringPointsFromThirtyShouldUpdatePointsToForty(PlayerSide scorerSide)
        {
            var scorer = new MatchScoreModelWrapper(match, scorerSide);

            scorer.Points = TennisPoint.Thirty;
            matchScoreCalculationService.Scoring(match, scorerSide);

            Assert.Equal(TennisPoint.Forty, scorer.Points);
        }

        [Theory]
        [MemberData(nameof(AllSides))]
        public void ScoringPointsFromFortyShouldIncreaseGames(PlayerSide scorerSide)
        {
            var scorer = new MatchScoreModelWrapper(match, scorerSide);

            scorer.Points = TennisPoint.Forty;
            matchScoreCalculationService.Scoring(match, scorerSide);

            Assert.Equal(1, scorer.Games);
        }

        [Theory]
        [MemberData(nameof(AllSides))]
        public void ScoringAtDeuceShouldNotIncreaseGames(PlayerSide scorerSide)
        {
            var scorer = new MatchScoreModelWrapper(match, scorerSide);
            var opponentSide = PlayerSideProcessor.OpponentOf(scorerSide);
            var opponent = new MatchScoreModelWrapper(match, opponentSide);

            scorer.Points = TennisPoint.Forty;
            opponent.Points = TennisPoint.Forty;

            matchScoreCalculationService.Scoring(match, scorerSide);

            Assert.Equal(0, scorer.Games);
        }

        [Theory]
        [MemberData(nameof(AllSides))]
        public void ScoringAtDeuceWithNoAdvantageShouldNotIncreaseGames(PlayerSide scorerSide)
        {
            var scorer = new MatchScoreModelWrapper(match, scorerSide);
            var opponent = new MatchScoreModelWrapper(match, PlayerSideProcessor.OpponentOf(scorerSide));

            scorer.Points = TennisPoint.Forty;
            opponent.Points = TennisPoint.Forty;
            scorer.HasAdvantage = false;

            matchScoreCalculationService.Scoring(match, scorerSide);

            Assert.Equal(0, scorer.Games);
        }

        [Theory]
        [MemberData(nameof(AllSides))]
        public void ScoringAtDeuceWithAdvantageShouldWinGame(PlayerSide scorerSide)
        {
            var scorer = new MatchScoreModelWrapper(match, scorerSide);
            var opponent = new MatchScoreModelWrapper(match, PlayerSideProcessor.OpponentOf(scorerSide));

            scorer.Points = TennisPoint.Forty;
            opponent.Points = TennisPoint.Forty;
            scorer.HasAdvantage = true;

            matchScoreCalculationService.Scoring(match, scorerSide);

            Assert.Equal(1, scorer.Games);
        }

        [Theory]
        [MemberData(nameof(AllSides))]
        public void ScoringAtDeuceWithAdvantageShouldResetAdvantages(PlayerSide scorerSide)
        {
            var scorer = new MatchScoreModelWrapper(match, scorerSide);
            var opponent = new MatchScoreModelWrapper(match, PlayerSideProcessor.OpponentOf(scorerSide));

            scorer.Points = TennisPoint.Forty;
            opponent.Points = TennisPoint.Forty;
            scorer.HasAdvantage = true;

            matchScoreCalculationService.Scoring(match, scorerSide);

            Assert.False(scorer.HasAdvantage);
            Assert.False(opponent.HasAdvantage);
        }

        [Theory]
        [MemberData(nameof(AllSides))]
        public void ScoringGamesAt6_5ShouldStartTiebreak(PlayerSide scorerSide)
        {
            var scorer = new MatchScoreModelWrapper(match, scorerSide);
            var opponent = new MatchScoreModelWrapper(match, PlayerSideProcessor.OpponentOf(scorerSide));

            scorer.Points = TennisPoint.Forty;
            opponent.Points = TennisPoint.Love;
            scorer.Games = 5;
            opponent.Games = 6;

            matchScoreCalculationService.Scoring(match, scorerSide);

            Assert.True(match.Tiebreak);
        }

        [Theory]
        [MemberData(nameof(AllSides))]
        public void ScoringGamesAt5_5ShouldNotStartTiebreak(PlayerSide scorerSide)
        {
            var scorer = new MatchScoreModelWrapper(match, scorerSide);
            var opponent = new MatchScoreModelWrapper(match, PlayerSideProcessor.OpponentOf(scorerSide));

            scorer.Points = TennisPoint.Forty;
            opponent.Points = TennisPoint.Love;
            scorer.Games = 5;
            opponent.Games = 5;

            matchScoreCalculationService.Scoring(match, scorerSide);

            Assert.False(match.Tiebreak);
        }

        [Theory]
        [MemberData(nameof(AllSides))]
        public void ScoringGamesAt6_4ShouldIncreaseSets(PlayerSide scorerSide)
        {
            var scorer = new MatchScoreModelWrapper(match, scorerSide);
            var opponent = new MatchScoreModelWrapper(match, PlayerSideProcessor.OpponentOf(scorerSide));

            scorer.Points = TennisPoint.Forty;
            opponent.Points = TennisPoint.Love;
            scorer.Games = 5;
            opponent.Games = 4;
            scorer.Sets = 0;

            matchScoreCalculationService.Scoring(match, scorerSide);

            Assert.Equal(1, scorer.Sets);
        }
    }
}
