using System.Collections.Generic;

namespace Kontur.GameStats.Server.Dtos
{
    public class ServerStatsDto
    {
        public int TotalMatchesPlayed { get; set; }
        public int MaximumMatchesPerDay { get; set; }
        public double AverageMatchesPerDay { get; set; }
        public int MaximumPopulation { get; set; }
        public double AveragePopulation { get; set; }
        public List<string> Top5GameModes { get; set; }
        public List<string> Top5Maps { get; set; }
    }
}