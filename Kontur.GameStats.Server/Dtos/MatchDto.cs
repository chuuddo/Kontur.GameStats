using System;

namespace Kontur.GameStats.Server.Dtos
{
    public class MatchDto
    {
        public string Server { get; set; }
        public DateTime Timestamp { get; set; }
        public MatchResultsDto Results { get; set; }
    }
}