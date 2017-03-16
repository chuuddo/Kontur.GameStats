using System;
using System.Threading.Tasks;
using System.Web.Http;
using Kontur.GameStats.Server.Dtos;
using Kontur.GameStats.Server.Infrastructure;
using MediatR;

namespace Kontur.GameStats.Server.Features.Matches
{
    public class MatchesController : ApiController
    {
        private readonly IMediator _mediator;

        public MatchesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("servers/{endpoint}/matches/{timestamp}")]
        public async Task<IHttpActionResult> GetMatchResults(string endpoint, DateTimeOffset timestamp)
        {
            AddModelErrorIfTimestampIsNotUtc(timestamp);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var content = await _mediator.Send(new GetMatchResultsQuery(endpoint, timestamp.UtcDateTime));
            if (content == null) return NotFound();
            return Ok(content);
        }

        [Route("servers/{endpoint}/matches/{timestamp}")]
        public async Task<IHttpActionResult> PutMatchResults(string endpoint, DateTimeOffset timestamp, MatchResultsDto matchResults)
        {
            AddModelErrorIfTimestampIsNotUtc(timestamp);
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(new PutMatchResultsCommand(endpoint, timestamp.UtcDateTime, matchResults));
                    return Ok();
                }
                catch (ValidationException e)
                {
                    ModelState.AddModelError(e.Property, e.Message);
                }
            }
            return BadRequest(ModelState);
        }

        private void AddModelErrorIfTimestampIsNotUtc(DateTimeOffset timestamp)
        {
            if (timestamp.DateTime != timestamp.UtcDateTime)
                ModelState.AddModelError("Timestamp", "Timestamp is not UTC.");
        }
    }
}