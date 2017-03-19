using System;
using System.Data.Entity;
using Fclp;
using Kontur.GameStats.Server.Configuration;
using Kontur.GameStats.Server.Data;
using Microsoft.Owin.Hosting;
using Serilog;

namespace Kontur.GameStats.Server
{
    public class EntryPoint
    {
        private static IDisposable _webapp;

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message}{NewLine}")
                .WriteTo.RollingFile("Logs/log-{Date}.txt")
                .CreateLogger();
            Log.Logger.Information($"Logger initialized. Verbose log files is located at {AppDomain.CurrentDomain.BaseDirectory}Logs\\");

            Console.TreatControlCAsInput = false;
            Console.CancelKeyPress += (s, e) =>
            {
                Log.Logger.Information("Stoping web server...");
                _webapp?.Dispose();
                Log.Logger.Information("Web server stoped.");
                Environment.Exit(0);
            };

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

            Log.Logger.Information("Initializing database...");
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
            new ApplicationDbContext().Database.Initialize(true);
            Log.Logger.Information("Database initialized.");

            Log.Logger.Information("Starting web server...");
            _webapp = WebApp.Start<Startup>(commandLineParser.Object.Prefix);
            Log.Logger.Information($"Server started at {commandLineParser.Object.Prefix} - press Ctrl+C to quit.");
            while (true)
            {
                Console.ReadKey(true);
            }
        }

        private class Options
        {
            public string Prefix { get; set; }
        }
    }
}