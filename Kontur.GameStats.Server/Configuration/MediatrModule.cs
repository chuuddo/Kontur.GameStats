using System.Collections.Generic;
using Autofac;
using Autofac.Features.Variance;
using MediatR;

namespace Kontur.GameStats.Server.Configuration
{
    public class MediatrModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();
            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.ResolveOptional(t);
            });
            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>) c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });
            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IAsyncRequestHandler<>));
            builder.RegisterAssemblyTypes(ThisAssembly).AsClosedTypesOf(typeof(IAsyncRequestHandler<,>));
        }
    }
}