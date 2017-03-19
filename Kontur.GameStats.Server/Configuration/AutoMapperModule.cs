using Autofac;
using AutoMapper;

namespace Kontur.GameStats.Server.Configuration
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MapperConfiguration(cfg => cfg.AddProfiles(ThisAssembly))).AsSelf().SingleInstance();
            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve)).As<IMapper>().SingleInstance();
        }
    }
}