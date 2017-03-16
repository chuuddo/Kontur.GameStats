using System.Collections.Generic;
using FluentValidation.TestHelper;
using Kontur.GameStats.Server.Dtos;
using NUnit.Framework;

namespace Kontur.GameStats.Server.Test.Dtos
{
    [TestFixture]
    [Category("Unit")]
    public class MatchResultsDtoValidatorTests
    {
        [SetUp]
        public void Setup()
        {
            _validator = new MatchResultsDto.Validator();
        }

        private MatchResultsDto.Validator _validator;

        [Test]
        public void Should_have_child_validator_for_scoreboad_collection()
        {
            _validator.ShouldHaveChildValidator(x => x.Scoreboard, typeof(ScoreDto.Validator));
        }

        [Test]
        public void Should_have_error_when_fraglimit_is_not_positive()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.FragLimit, 0);
            _validator.ShouldHaveValidationErrorFor(x => x.FragLimit, -1);
        }

        [Test]
        public void Should_have_error_when_gamemode_is_null_or_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.GameMode, null as string);
            _validator.ShouldHaveValidationErrorFor(x => x.GameMode, string.Empty);
            _validator.ShouldHaveValidationErrorFor(x => x.GameMode, "");
        }

        [Test]
        public void Should_have_error_when_instance_is_null()
        {
            var validationResult = _validator.Validate(null);

            Assert.False(validationResult.IsValid);
            Assert.AreEqual("MatchResultsDto", validationResult.Errors[0].PropertyName);
        }

        [Test]
        public void Should_have_error_when_map_is_null_or_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Map, null as string);
            _validator.ShouldHaveValidationErrorFor(x => x.Map, string.Empty);
            _validator.ShouldHaveValidationErrorFor(x => x.Map, "");
        }

        [Test]
        public void Should_have_error_when_scoreboard_is_null_or_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Scoreboard, null as List<ScoreDto>);
            _validator.ShouldHaveValidationErrorFor(x => x.Scoreboard, new List<ScoreDto>());
        }

        [Test]
        public void Should_have_error_when_timeelapsed_is_not_positive()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.TimeElapsed, 0);
            _validator.ShouldHaveValidationErrorFor(x => x.TimeElapsed, -1);
        }

        [Test]
        public void Should_have_error_when_timelimit_is_not_positive()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.TimeLimit, 0);
            _validator.ShouldHaveValidationErrorFor(x => x.TimeLimit, -1);
        }
    }
}