using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Kontur.GameStats.Server.Dtos;
using MediatR;

namespace Kontur.GameStats.Server.Features.Reports
{
    [Route("reports/{action}/{count:int?}")]
    public class ReportsController : ApiController
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private static int GetCroppedCount(int count) => count > 50 ? 50 : count;

        [ActionName("recent-matches")]
        public async Task<IHttpActionResult> GetRecentMatches(int count = 5)
        {
            var content = count > 0
                ? await _mediator.Send(new GetRecentMatchesQuery(GetCroppedCount(count)))
                : new List<MatchDto>();
            return Ok(content);
        }

        [ActionName("best-players")]
        public async Task<IHttpActionResult> GetBestPlayers(int count = 5)
        {
            var content = count > 0
                ? await _mediator.Send(new GetBestPlayersQuery(GetCroppedCount(count)))
                : new List<BestPlayerDto>();
            return Ok(content);
        }

        [ActionName("popular-servers")]
        public async Task<IHttpActionResult> GetPopularServers(int count = 5)
        {
            var content = count > 0
                ? await _mediator.Send(new GetPopularServersQuery(GetCroppedCount(count)))
                : new List<PopularServerDto>();
            return Ok(content);
        }
    }
}