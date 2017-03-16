using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using FakeItEasy;
using Kontur.GameStats.Server.Dtos;
using Kontur.GameStats.Server.Features.Servers;
using MediatR;
using NUnit.Framework;

namespace Kontur.GameStats.Server.Test.Features.Servers
{
    [TestFixture]
    [Category("Unit")]
    public class ServersControllerTests
    {
        [SetUp]
        public void Setup()
        {
            _fakeMediator = A.Fake<IMediator>();
            _serversController = new ServersController(_fakeMediator);
        }

        private ServersController _serversController;
        private IMediator _fakeMediator;

        [Test]
        public async Task GetAllServers_should_return_empty_list_when_servers_does_not_exists()
        {
            A.CallTo(() => _fakeMediator.Send(A<GetAllServersQuery>._, A<CancellationToken>._)).Returns(new List<ServerDto>());

            var result = await _serversController.GetAllServers() as OkNegotiatedContentResult<List<ServerDto>>;

            Assert.NotNull(result);
            Assert.AreEqual(0, result.Content.Count);
        }

        [Test]
        public async Task GetAllServers_should_return_list_of_servers_when_any_server_exists()
        {
            var serverDtos = new List<ServerDto>
            {
                new ServerDto
                {
                    Endpoint = string.Empty,
                    Info = new ServerInfoDto {Name = string.Empty, GameModes = new List<string> {string.Empty}}
                }
            };
            A.CallTo(() => _fakeMediator.Send(A<GetAllServersQuery>._, A<CancellationToken>._)).Returns(serverDtos);

            var result = await _serversController.GetAllServers() as OkNegotiatedContentResult<List<ServerDto>>;

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Content.Count);
            Assert.AreEqual(1, result.Content[0].Info.GameModes.Count);
            Assert.AreEqual(string.Empty, result.Content[0].Endpoint);
            Assert.AreEqual(string.Empty, result.Content[0].Info.Name);
        }

        [Test]
        public async Task GetServer_should_return_not_found_when_server_does_not_exists()
        {
            A.CallTo(() => _fakeMediator.Send(A<GetServerInfoQuery>._, A<CancellationToken>._)).Returns(null as ServerInfoDto);

            var result = await _serversController.GetServer(string.Empty);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetServer_should_return_server_info_when_server_exists()
        {
            var serverInfoDto = new ServerInfoDto {Name = string.Empty, GameModes = new List<string> {string.Empty}};
            A.CallTo(() => _fakeMediator.Send(A<GetServerInfoQuery>._, A<CancellationToken>._)).Returns(serverInfoDto);

            var result = await _serversController.GetServer(string.Empty) as OkNegotiatedContentResult<ServerInfoDto>;

            Assert.NotNull(result);
            Assert.AreEqual(string.Empty, result.Content.Name);
            Assert.AreEqual(1, result.Content.GameModes.Count);
        }

        [Test]
        public async Task PutServer_should_return_bad_request_when_model_is_not_valid()
        {
            _serversController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = await _serversController.PutServer(string.Empty, null);

            A.CallTo(() => _fakeMediator.Send(A<PutServerInfoCommand>._, A<CancellationToken>._)).MustNotHaveHappened();
            Assert.IsInstanceOf<InvalidModelStateResult>(result);
        }

        [Test]
        public async Task PutServer_should_return_ok_when_model_is_valid()
        {
            var result = await _serversController.PutServer(string.Empty, null);

            A.CallTo(() => _fakeMediator.Send(A<PutServerInfoCommand>._, A<CancellationToken>._)).MustHaveHappened();
            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}