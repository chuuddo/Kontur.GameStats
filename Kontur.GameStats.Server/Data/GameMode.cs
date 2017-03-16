using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kontur.GameStats.Server.Data
{
    public class GameMode
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<Server> Servers { get; set; } = new HashSet<Server>();
        public virtual ICollection<Match> Matches { get; set; } = new HashSet<Match>();
    }
}