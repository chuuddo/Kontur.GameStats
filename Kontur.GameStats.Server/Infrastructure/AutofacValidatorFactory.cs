using System;
using Autofac;
using FluentValidation;

namespace Kontur.GameStats.Server.Infrastructure
{
    public class AutofacValidatorFactory : ValidatorFactoryBase
    {
        private readonly IComponentContext _context;

        public AutofacValidatorFactory(IComponentContext context)
        {
            _context = context;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return _context.ResolveOptional(validatorType) as IValidator;
        }
    }
}