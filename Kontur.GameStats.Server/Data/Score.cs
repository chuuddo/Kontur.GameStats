using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kontur.GameStats.Server.Data
{
    public class Score
    {
        [Column(Order = 0)]
        [Key]
        public int MatchId { get; set; }

        [Column(Order = 1)]
        [Key]
        public int PlayerId { get; set; }

        public double ScoreboardPercent { get; set; }
        public int Frags { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public virtual Player Player { get; set; }
        public virtual Match Match { get; set; }
    }
}