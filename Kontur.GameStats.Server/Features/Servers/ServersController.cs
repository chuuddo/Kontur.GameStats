﻿using System.Threading.Tasks;
using System.Web.Http;
using Kontur.GameStats.Server.Dtos;
using MediatR;

namespace Kontur.GameStats.Server.Features.Servers
{
    public class ServersController : ApiController
    {
        private readonly IMediator _mediator;

        public ServersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("servers/{endpoint}/info")]
        public async Task<IHttpActionResult> GetServer(string endpoint)
        {
            var content = await _mediator.Send(new GetServerInfoQuery(endpoint));
            if (content == null) return NotFound();
            return Ok(content);
        }

        [Route("servers/info")]
        public async Task<IHttpActionResult> GetAllServers()
        {
            var content = await _mediator.Send(new GetAllServersQuery());
            return Ok(content);
        }

        [Route("servers/{endpoint}/info")]
        public async Task<IHttpActionResult> PutServer(string endpoint, ServerInfoDto serverInfo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _mediator.Send(new PutServerInfoCommand(endpoint, serverInfo));
            return Ok();
        }
    }
}