using System.Web.Http.Validation;
using Autofac;
using FluentValidation;
using FluentValidation.WebApi;
using Kontur.GameStats.Server.Infrastructure;

namespace Kontur.GameStats.Server.Configuration
{
    public class FluentValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            AssemblyScanner.FindValidatorsInAssembly(ThisAssembly)
                .ForEach(x => builder.RegisterType(x.ValidatorType).As(x.InterfaceType).SingleInstance());
            builder.RegisterType<FluentValidationModelValidatorProvider>().As<ModelValidatorProvider>();
            builder.RegisterType<AutofacValidatorFactory>().As<IValidatorFactory>().SingleInstance();
        }
    }
}