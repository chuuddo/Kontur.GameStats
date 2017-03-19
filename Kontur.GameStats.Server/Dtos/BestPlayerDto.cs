using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Dtos
{
    public class BestPlayerDto
    {
        public string Name { get; set; }
        public double KillToDeathRatio { get; set; }
    }
}
