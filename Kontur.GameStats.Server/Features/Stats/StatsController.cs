using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using WebApi.OutputCache.V2;

namespace Kontur.GameStats.Server.Features.Stats
{
    [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
    public class StatsController : ApiController
    {
        private readonly IMediator _mediator;

        public StatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("servers/{endpoint}/stats")]
        public async Task<IHttpActionResult> GetServerStats(string endpoint)
        {
            var content = await _mediator.Send(new GetServerStatsQuery(endpoint));
            if (content == null) return NotFound();
            return Ok(content);
        }

        [Route("players/{name}/stats")]
        public async Task<IHttpActionResult> GetPlayerStats(string name)
        {
            var content = await _mediator.Send(new GetPlayerStatsQuery(name));
            if (content == null) return NotFound();
            return Ok(content);
        }
    }
}