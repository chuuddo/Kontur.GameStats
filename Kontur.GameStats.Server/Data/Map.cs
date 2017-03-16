using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kontur.GameStats.Server.Data
{
    public class Map
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<Match> Matches { get; set; } = new HashSet<Match>();
    }
}