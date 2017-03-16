using FluentValidation;
using FluentValidation.Results;

namespace Kontur.GameStats.Server.Dtos
{
    public class ScoreDto
    {
        public string Name { get; set; }
        public int Frags { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }

        public class Validator : AbstractValidator<ScoreDto>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty();
            }

            public new ValidationResult Validate(ScoreDto instance)
            {
                return instance == null
                    ? new ValidationResult(new[] {new ValidationFailure(nameof(ScoreDto), "Should be not null.")})
                    : base.Validate(instance);
            }
        }
    }
}