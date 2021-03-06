﻿using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Autofac;
using Autofac.Integration.WebApi;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Infrastructure;
using Microsoft.Owin.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using LoggerFactory = SerilogWeb.Owin.LoggerFactory;

namespace Kontur.GameStats.Server.Configuration
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerRequest();
            var container = builder.Build();

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new DoubleFormatConverter());
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            config.MessageHandlers.Add(new PlainTextToJsonHandler());
            config.Services.Add(typeof(IExceptionLogger), new SerilogExceptionLogger());
            app.SetLoggerFactory(new LoggerFactory());
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }
    }
}