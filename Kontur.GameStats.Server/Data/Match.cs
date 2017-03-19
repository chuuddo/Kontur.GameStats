using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kontur.GameStats.Server.Data
{
    public class Match
    {
        public int Id { get; set; }

        [Index]
        [Column(TypeName = "DateTime")]
        public DateTime Timestamp { get; set; }

        public Map Map { get; set; }
        public GameMode GameMode { get; set; }
        public int FragLimit { get; set; }
        public int TimeLimit { get; set; }
        public double TimeElapsed { get; set; }
        public virtual ICollection<Score> ScoreBoard { get; set; } = new HashSet<Score>();
        public virtual Server Server { get; set; }
    }
}