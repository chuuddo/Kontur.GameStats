using System.Collections.Generic;
using FluentValidation.TestHelper;
using Kontur.GameStats.Server.Dtos;
using NUnit.Framework;

namespace Kontur.GameStats.Server.Test.Dtos
{
    [TestFixture]
    [Category("Unit")]
    public class ServerInfoDtoValidatorTests
    {
        [SetUp]
        public void Setup()
        {
            _validator = new ServerInfoDto.Validator();
        }

        private ServerInfoDto.Validator _validator;

        [Test]
        public void Should_have_error_when_gamemodes_contains_duplicate_items()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.GameModes, new List<string> {"Duplicate", "Duplicate"});
            _validator.ShouldHaveValidationErrorFor(x => x.GameModes, new List<string> {"Duplicate", "DUPLICATE"});
        }

        [Test]
        public void Should_have_error_when_gamemodes_contains_null_or_empty_items()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.GameModes, new List<string> {"Mode1", null});
            _validator.ShouldHaveValidationErrorFor(x => x.GameModes, new List<string> {"Mode1", string.Empty});
            _validator.ShouldHaveValidationErrorFor(x => x.GameModes, new List<string> {"Mode1", ""});
        }

        [Test]
        public void Should_have_error_when_gamemodes_is_null_or_empty()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.GameModes, null as List<string>);
            _validator.ShouldHaveValidationErrorFor(x => x.GameModes, new List<string>());
        }

        [Test]
        public void Should_have_error_when_instance_is_null()
        {
            var validationResult = _validator.Validate(null);

            Assert.False(validationResult.IsValid);
            Assert.AreEqual("ServerInfoDto", validationResult.Errors[0].PropertyName);
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