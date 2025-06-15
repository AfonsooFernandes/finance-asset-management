using FinanceTracker.Models;
using FinanceTracker.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FinanceTracker.TestsApi1
{
    [TestFixture]
    public class ImovelArrendadoServiceTests
    {
        private Mock<HttpMessageHandler> _handlerMock;
        private HttpClient _httpClient;
        private ImovelArrendadoService _service;

        [SetUp]
        public void SetUp()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };
            _service = new ImovelArrendadoService(_httpClient);
        }
        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
        }

        [Test]
        public async Task GetAllImoveis_Success_ReturnsImoveis()
        {
            // Arrange
            var imoveis = new List<ImovelArrendadoDto>
            {
                new ImovelArrendadoDto { Id = 1, AtivoId = 1, Designacao = "Apartamento T2", Localizacao = "Lisboa", ValorImovel = 200000.0, ValorRenda = 1000.0, ValorCondominio = 100.0, OutrasDespesas = 50.0 },
                new ImovelArrendadoDto { Id = 2, AtivoId = 2, Designacao = "Casa T3", Localizacao = "Porto", ValorImovel = 300000.0, ValorRenda = 1500.0, ValorCondominio = 200.0, OutrasDespesas = 100.0 }
            };
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(imoveis), Encoding.UTF8, "application/json")
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetAllImoveis();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().ValorRenda, Is.EqualTo(1000.0));
            Assert.That(result.First().Designacao, Is.EqualTo("Apartamento T2"));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("api/imoveis")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task GetImovelById_Success_ReturnsImovel()
        {
            // Arrange
            var imovel = new ImovelArrendadoDto { Id = 1, AtivoId = 1, Designacao = "Apartamento T2", Localizacao = "Lisboa", ValorImovel = 200000.0, ValorRenda = 1000.0, ValorCondominio = 100.0, OutrasDespesas = 50.0 };
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(imovel), Encoding.UTF8, "application/json")
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetImovelById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValorRenda, Is.EqualTo(1000.0));
            Assert.That(result.Designacao, Is.EqualTo("Apartamento T2"));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("api/imoveis/1")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task GetImovelByAtivoId_Success_ReturnsImovel()
        {
            // Arrange
            var imovel = new ImovelArrendadoDto { Id = 1, AtivoId = 1, Designacao = "Apartamento T2", Localizacao = "Lisboa", ValorImovel = 200000.0, ValorRenda = 1000.0, ValorCondominio = 100.0, OutrasDespesas = 50.0 };
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(imovel), Encoding.UTF8, "application/json")
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetImovelByAtivoId(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ValorRenda, Is.EqualTo(1000.0));
            Assert.That(result.Designacao, Is.EqualTo("Apartamento T2"));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("api/imoveis/ativo/1")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task CreateImovel_Success_ReturnsSuccessMessage()
        {
            // Arrange
            var imovel = new ImovelArrendadoDto { Id = 1, AtivoId = 1, Designacao = "Apartamento T2", Localizacao = "Lisboa", ValorImovel = 200000.0, ValorRenda = 1000.0, ValorCondominio = 100.0, OutrasDespesas = 50.0 };
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.CreateImovel(imovel);

            // Assert
            Assert.That(result, Is.EqualTo("Imóvel arrendado criado com sucesso."));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Post && req.RequestUri.ToString().EndsWith("api/imoveis")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task UpdateImovel_Success_ReturnsSuccessMessage()
        {
            // Arrange
            var imovel = new ImovelArrendadoDto { Id = 1, AtivoId = 1, Designacao = "Apartamento T2", Localizacao = "Lisboa", ValorImovel = 200000.0, ValorRenda = 1000.0, ValorCondominio = 100.0, OutrasDespesas = 50.0 };
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.UpdateImovel(1, imovel);

            // Assert
            Assert.That(result, Is.EqualTo("Imóvel arrendado atualizado com sucesso."));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Put && req.RequestUri.ToString().EndsWith("api/imoveis/1")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task DeleteImovel_Success_ReturnsSuccessMessage()
        {
            // Arrange
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.DeleteImovel(1);

            // Assert
            Assert.That(result, Is.EqualTo("Imóvel arrendado removido com sucesso."));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Delete && req.RequestUri.ToString().EndsWith("api/imoveis/1")), ItExpr.IsAny<CancellationToken>());
        }
    }
}