using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using FakeItEasy;
using Kontur.GameStats.Server.Dtos;
using Kontur.GameStats.Server.Features.Matches;
using Kontur.GameStats.Server.Infrastructure;
using MediatR;
using NUnit.Framework;

namespace Kontur.GameStats.Server.Test.Features.Matches
{
    [TestFixture]
    [Category("Unit")]
    public class MatchesControllerTests
    {
        [SetUp]
        public void Setup()
        {
            _fakeMediator = A.Fake<IMediator>();
            _matchesController = new MatchesController(_fakeMediator);
        }

        private MatchesController _matchesController;
        private IMediator _fakeMediator;

        [Test]
        public async Task GetMatchResults_should_return_bad_request_when_timestamp_is_not_utc()
        {
            var result = await _matchesController.GetMatchResults(string.Empty, new DateTimeOffset(DateTime.Now));

            A.CallTo(() => _fakeMediator.Send(A<GetMatchResultsQuery>._, A<CancellationToken>._)).MustNotHaveHappened();
            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public async Task GetMatchResults_should_return_match_results_when_match_exists()
        {
            var matchResultsDto = new MatchResultsDto {Scoreboard = new List<ScoreDto> {new ScoreDto()}};
            A.CallTo(() => _fakeMediator.Send(A<GetMatchResultsQuery>._, A<CancellationToken>._)).Returns(matchResultsDto);

            var result =
                await _matchesController.GetMatchResults(string.Empty, new DateTimeOffset(DateTime.UtcNow)) as
                    OkNegotiatedContentResult<MatchResultsDto>;

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Content.Scoreboard.Count);
        }

        [Test]
        public async Task GetMatchResults_should_return_not_found_when_server_or_match_does_not_exists()
        {
            A.CallTo(() => _fakeMediator.Send(A<GetMatchResultsQuery>._, A<CancellationToken>._)).Returns(null as MatchResultsDto);

            var result = await _matchesController.GetMatchResults(string.Empty, new DateTimeOffset(DateTime.UtcNow));

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task PutMatchResults_should_return_bad_request_when_model_is_not_valid()
        {
            _matchesController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = await _matchesController.PutMatchResults(string.Empty, new DateTimeOffset(DateTime.UtcNow), new MatchResultsDto());

            A.CallTo(() => _fakeMediator.Send(A<PutMatchResultsCommand>._, A<CancellationToken>._)).MustNotHaveHappened();
            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public async Task PutMatchResults_should_return_bad_request_when_timestamp_is_not_utc()
        {
            var result = await _matchesController.PutMatchResults(string.Empty, new DateTimeOffset(DateTime.Now), new MatchResultsDto());

            A.CallTo(() => _fakeMediator.Send(A<PutMatchResultsCommand>._, A<CancellationToken>._)).MustNotHaveHappened();
            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public async Task PutMatchResults_should_return_bad_request_when_validation_exception_thrown()
        {
            A.CallTo(() => _fakeMediator.Send(A<PutMatchResultsCommand>._, A<CancellationToken>._))
                .Throws(new ValidationException(string.Empty, string.Empty));

            var result = await _matchesController.PutMatchResults(string.Empty, new DateTimeOffset(DateTime.UtcNow), new MatchResultsDto());

            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public async Task PutMatchResults_should_return_ok_when_model_is_valid()
        {
            var result = await _matchesController.PutMatchResults(string.Empty, new DateTimeOffset(DateTime.UtcNow), new MatchResultsDto());

            A.CallTo(() => _fakeMediator.Send(A<PutMatchResultsCommand>._, A<CancellationToken>._)).MustHaveHappened();
            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}