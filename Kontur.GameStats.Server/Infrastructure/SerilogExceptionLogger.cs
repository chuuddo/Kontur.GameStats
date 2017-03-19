using System.Text;
using System.Web.Http.ExceptionHandling;

namespace Kontur.GameStats.Server.Infrastructure
{
    public class SerilogExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            var message = new StringBuilder();
            message.AppendLine($"{context.Request.Method} {context.Request.RequestUri}");
            message.AppendLine($"Exception Message: {context.Exception.Message}");
            var requestBody = context.Request.Content.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(requestBody))
                message.Append($"Request Body: {requestBody}");

            Serilog.Log.Logger.Error(context.Exception, message.ToString());
        }
    }
}