using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kontur.GameStats.Server.Data
{
    public class Server
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        public string Endpoint { get; set; }

        public string Name { get; set; }
        public virtual ICollection<GameMode> GameModes { get; set; } = new HashSet<GameMode>();
        public virtual ICollection<Match> Matches { get; set; } = new HashSet<Match>();
    }
}