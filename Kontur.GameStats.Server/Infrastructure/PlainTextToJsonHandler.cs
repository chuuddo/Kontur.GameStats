using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;

namespace Kontur.GameStats.Server.Infrastructure
{
    public class PlainTextToJsonHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if ((request.Method == HttpMethod.Put || request.Method == HttpMethod.Post) &&
                (request.Content.Headers.ContentType == null || request.Content.Headers.ContentType.MediaType == "text/plain"))
            {
                var body = (await request.Content.ReadAsStringAsync()).Trim();
                if (body.StartsWith("{") && body.EndsWith("}") || body.StartsWith("[") && body.EndsWith("]"))
                {
                    request.Content.Headers.ContentType.MediaType = "application/json";
                }
                else
                {
                    var content = new StringContent(SerializeObject(new {message = "Request body is not JSON."}), Encoding.UTF8, "application/json");
                    return new HttpResponseMessage(HttpStatusCode.BadRequest) {Content = content};
                }
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}