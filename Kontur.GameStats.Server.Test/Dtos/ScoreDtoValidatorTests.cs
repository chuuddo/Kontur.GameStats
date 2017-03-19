using FluentValidation.TestHelper;
using Kontur.GameStats.Server.Dtos;
using NUnit.Framework;

namespace Kontur.GameStats.Server.Test.Dtos
{
    [TestFixture]
    [Category("Unit")]
    public class ScoreDtoValidatorTests
    {
        [SetUp]
        public void Setup()
        {
            _validator = new ScoreDto.Validator();
        }

        private ScoreDto.Validator _validator;

        [Test]
        public void Should_have_error_when_instance_is_null()
        {
            var validationResult = _validator.Validate(null);

            Assert.False(validationResult.IsValid);
            Assert.AreEqual("ScoreDto", validationResult.Errors[0].PropertyName);
        }

        [Test]
        public void Should_have_error_when_name_is_null_or_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Name, null as string);
            _validator.ShouldHaveValidationErrorFor(x => x.Name, string.Empty);
            _validator.ShouldHaveValidationErrorFor(x => x.Name, "");
        }
    }
}