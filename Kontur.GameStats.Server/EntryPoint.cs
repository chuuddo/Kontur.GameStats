using System;
using Fclp;
using Microsoft.Owin.Hosting;

namespace Kontur.GameStats.Server
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            Console.TreatControlCAsInput = false;
            Console.CancelKeyPress += (s, e) => { Environment.Exit(0); };

            var commandLineParser = new FluentCommandLineParser<Options>();
            commandLineParser
                .Setup(options => options.Prefix)
                .As("prefix")
                .SetDefault("http://+:8080/")
                .WithDescription("HTTP prefix to listen on");
            commandLineParser
                .SetupHelp("h", "help")
                .WithHeader($"{AppDomain.CurrentDomain.FriendlyName} [--prefix <prefix>]")
                .Callback(text => Console.WriteLine(text));
            if (commandLineParser.Parse(args).HelpCalled)
                return;

            using (WebApp.Start<Startup>(commandLineParser.Object.Prefix))
            {
                while (true) Console.ReadKey(true);
            }
        }

        private class Options
        {
            public string Prefix { get; set; }
        }
    }
}