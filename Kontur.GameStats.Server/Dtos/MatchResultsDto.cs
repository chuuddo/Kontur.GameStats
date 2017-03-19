using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace Kontur.GameStats.Server.Dtos
{
    public class MatchResultsDto
    {
        public string Map { get; set; }
        public string GameMode { get; set; }
        public int FragLimit { get; set; }
        public int TimeLimit { get; set; }
        public double TimeElapsed { get; set; }
        public List<ScoreDto> Scoreboard { get; set; }

        public class Validator : AbstractValidator<MatchResultsDto>
        {
            public Validator()
            {
                RuleFor(x => x.Map).NotEmpty();
                RuleFor(x => x.GameMode).NotEmpty();
                RuleFor(x => x.FragLimit).GreaterThan(0);
                RuleFor(x => x.TimeLimit).GreaterThan(0);
                RuleFor(x => x.TimeElapsed).GreaterThan(0);
                RuleFor(x => x.Scoreboard)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty()
                    .SetCollectionValidator(new ScoreDto.Validator());
            }

            public new ValidationResult Validate(MatchResultsDto instance)
            {
                return instance == null
                    ? new ValidationResult(new[] {new ValidationFailure(nameof(MatchResultsDto), "Should be not null.")})
                    : base.Validate(instance);
            }
        }
    }
}