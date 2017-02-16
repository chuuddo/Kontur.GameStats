using System.Web.Http;

namespace Kontur.GameStats.Server.Controllers
{
    public class ServersController : ApiController
    {
        [Route("servers/{endpoint}/info")]
        public IHttpActionResult GetServer(string endpoint)
        {
            return Ok(endpoint);
        }
    }
}