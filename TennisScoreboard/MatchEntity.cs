using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TennisScoreboard
{
    public class MatchEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Player1Id { get; set; }
        public PlayerEntity Player1 { get; set; }

        public int Player2Id { get; set; }
        public PlayerEntity Player2 { get; set; }

        public int? WinnerId { get; set; } // может быть null, если матч не завершён
        public PlayerEntity Winner { get; set; }
    }
}
