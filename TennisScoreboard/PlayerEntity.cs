using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TennisScoreboard
{
    public class PlayerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<MatchEntity> MatchesAsPlayer1 { get; set; } = new List<MatchEntity>();
        public List<MatchEntity> MatchesAsPlayer2 { get; set; } = new List<MatchEntity>();
        public List<MatchEntity> MatchesWon { get; set; } = new List<MatchEntity>();

        public PlayerEntity() { }

        public PlayerEntity(string name)
        {
            Name = name;
        }
    }
}
