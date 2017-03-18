using System.Threading.Tasks;
using System.Web.Http;
using MediatR;

namespace Kontur.GameStats.Server.Features.Stats
{
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