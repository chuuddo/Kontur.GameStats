using System.Data.Entity;
using System.Diagnostics;

namespace Kontur.GameStats.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("Data source='Kontur.GameStats.Database.sdf';Max Database Size=4091;")
        {
#if DEBUG
            Database.Log = s => Debug.WriteLine(s);
#endif
        }

        public ApplicationDbContext(string connection) : base(connection)
        {
        }

        public DbSet<Server> Servers { get; set; }
        public DbSet<GameMode> GameModes { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Map> Maps { get; set; }
    }
}