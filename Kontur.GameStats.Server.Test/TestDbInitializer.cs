using System;
using System.Collections.Generic;
using System.Data.Entity;
using Kontur.GameStats.Server.Data;

namespace Kontur.GameStats.Server.Test
{
    public class TestDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);
            context.Database.Log = null;
            var gameModes = new List<GameMode>
            {
                new GameMode {Name = "DM"},
                new GameMode {Name = "TDM"}
            };
            var maps = new List<Map>
            {
                new Map {Name = "Map 1"},
                new Map {Name = "Map 2"}
            };
            var players = new List<Player>
            {
                new Player {Name = "Player 1"},
                new Player {Name = "Player 2"},
                new Player {Name = "Player 3"},
                new Player {Name = "Player 4"},
                new Player {Name = "Player 5"},
                new Player {Name = "Player 6"},
                new Player {Name = "Player 7"}
            };
            var server1 = new Data.Server {Endpoint = "localhost-1337"};
            var server2 = new Data.Server {Endpoint = "localhost-1448"};
            var server3 = new Data.Server {Endpoint = "localhost-2017"};
            var match1 = new Match
            {
                Timestamp = DateTime.Parse("2017-03-17T15:00:00Z").ToUniversalTime(),
                Map = maps[1],
                GameMode = gameModes[0],
                ScoreBoard =
                    new HashSet<Score>
                    {
                        new Score {Frags = 20, Kills = 20, Deaths = 2, ScoreboardPercent = 100, Player = players[0]},
                        new Score {Frags = 2, Kills = 2, Deaths = 20, ScoreboardPercent = 0, Player = players[1]}
                    }
            };
            var match2 = new Match
            {
                Timestamp = DateTime.Parse("2017-03-17T16:25:00Z").ToUniversalTime(),
                Map = maps[1],
                GameMode = gameModes[0],
                ScoreBoard =
                    new HashSet<Score>
                    {
                        new Score {Frags = 20, Kills = 20, Deaths = 0, ScoreboardPercent = 100, Player = players[3]},
                        new Score {Frags = 15, Kills = 15, Deaths = 5, ScoreboardPercent = 50, Player = players[0]},
                        new Score {Frags = 0, Kills = 0, Deaths = 30, ScoreboardPercent = 0, Player = players[2]}
                    }
            };
            var match3 = new Match
            {
                Timestamp = DateTime.Parse("2017-03-16T00:25:00Z").ToUniversalTime(),
                Map = maps[0],
                GameMode = gameModes[1],
                ScoreBoard =
                    new HashSet<Score>
                    {
                        new Score {Frags = 20, Kills = 20, Deaths = 5, ScoreboardPercent = 100, Player = players[1]},
                        new Score {Frags = 15, Kills = 15, Deaths = 12, ScoreboardPercent = 50, Player = players[0]},
                        new Score {Frags = 5, Kills = 5, Deaths = 23, ScoreboardPercent = 0, Player = players[2]}
                    }
            };
            var match4 = new Match
            {
                Timestamp = DateTime.Parse("2017-03-15T15:00:00Z").ToUniversalTime(),
                Map = maps[1],
                GameMode = gameModes[0],
                ScoreBoard =
                    new HashSet<Score>
                    {
                        new Score {Frags = 20, Kills = 21, Deaths = 3, ScoreboardPercent = 100, Player = players[0]},
                        new Score {Frags = 2, Kills = 2, Deaths = 21, ScoreboardPercent = 0, Player = players[1]}
                    }
            };
            var match5 = new Match
            {
                Timestamp = DateTime.Parse("2017-03-18T16:25:00Z").ToUniversalTime(),
                Map = maps[1],
                GameMode = gameModes[0],
                ScoreBoard =
                    new HashSet<Score>
                    {
                        new Score {Frags = 20, Kills = 20, Deaths = 0, ScoreboardPercent = 100, Player = players[3]},
                        new Score {Frags = 15, Kills = 15, Deaths = 5, ScoreboardPercent = 50, Player = players[0]},
                        new Score {Frags = 0, Kills = 0, Deaths = 30, ScoreboardPercent = 0, Player = players[2]}
                    }
            };
            for (int i = 0; i < 12; i++)
            {
                server3.Matches.Add(new Match
                {
                    Timestamp = DateTime.Parse($"2017-03-18T{i:D2}:25:00Z").ToUniversalTime(),
                    Map = maps[1],
                    GameMode = gameModes[0],
                    ScoreBoard =
                        new HashSet<Score>
                        {
                            new Score {Frags = 20, Kills = 20, Deaths = 0, ScoreboardPercent = 100, Player = players[4]},
                            new Score {Frags = 15, Kills = 15, Deaths = 5, ScoreboardPercent = 50, Player = players[5]},
                            new Score {Frags = 0, Kills = 0, Deaths = 30, ScoreboardPercent = 0, Player = players[6]}
                        }
                });
            }
            server1.Matches.Add(match1);
            server1.Matches.Add(match2);
            server1.Matches.Add(match3);
            server2.Matches.Add(match4);
            server2.Matches.Add(match5);
            context.Servers.Add(server1);
            context.Servers.Add(server2);
            context.Servers.Add(server3);
            context.SaveChanges();
        }
    }
}