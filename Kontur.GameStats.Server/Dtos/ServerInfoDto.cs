using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace Kontur.GameStats.Server.Dtos
{
    public class ServerInfoDto
    {
        public string Name { get; set; }
        public List<string> GameModes { get; set; }

        public class Validator : AbstractValidator<ServerInfoDto>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.GameModes)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty()
                    .Must(x => !x.Any(string.IsNullOrEmpty))
                    .WithMessage("'GameModes' should not contain empty items.")
                    .Must(x => x.Count == x.Distinct(StringComparer.OrdinalIgnoreCase).Count())
                    .WithMessage("'GameModes' should not contain duplicate items.");
            }

            public new ValidationResult Validate(ServerInfoDto instance)
            {
                return instance == null
                    ? new ValidationResult(new[] {new ValidationFailure(nameof(ServerInfoDto), "Should be not null.")})
                    : base.Validate(instance);
            }
        }
    }
}