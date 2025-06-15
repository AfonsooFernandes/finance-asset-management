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
    public class FundoInvestimentoServiceTests
    {
        private Mock<HttpMessageHandler> _handlerMock;
        private HttpClient _httpClient;
        private FundoInvestimentoService _service;

        [SetUp]
        public void SetUp()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };
            _service = new FundoInvestimentoService(_httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
        }

        [Test]
        public async Task GetAllFundos_Success_ReturnsFundos()
        {
            // Arrange
            var fundos = new List<FundoInvestimentoDto>
            {
                new FundoInvestimentoDto { Id = 1, AtivoId = 1, Nome = "Fundo A", Montante = 5000.0, TaxaJuro = 4.0 },
                new FundoInvestimentoDto { Id = 2, AtivoId = 2, Nome = "Fundo B", Montante = 10000.0, TaxaJuro = 5.0 }
            };
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(fundos), Encoding.UTF8, "application/json")
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetAllFundos();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Count(), Is.EqualTo(2));
            Assert.That(result?.First()?.Montante, Is.EqualTo(5000.0));
            Assert.That(result?.First()?.Nome, Is.EqualTo("Fundo A"));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("api/fundos")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task GetAllFundos_Failure_ReturnsEmptyList()
        {
            // Arrange
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetAllFundos();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetFundoById_Success_ReturnsFundo()
        {
            // Arrange
            var fundo = new FundoInvestimentoDto { Id = 1, AtivoId = 1, Nome = "Fundo A", Montante = 5000.0, TaxaJuro = 4.0 };
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(fundo), Encoding.UTF8, "application/json")
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetFundoById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Montante, Is.EqualTo(5000.0));
            Assert.That(result?.Nome, Is.EqualTo("Fundo A"));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("api/fundos/1")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task GetFundoById_Failure_ReturnsNull()
        {
            // Arrange
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetFundoById(1);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetFundoByAtivoId_Success_ReturnsFundo()
        {
            // Arrange
            var fundo = new FundoInvestimentoDto { Id = 1, AtivoId = 1, Nome = "Fundo A", Montante = 5000.0, TaxaJuro = 4.0 };
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(fundo), Encoding.UTF8, "application/json")
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetFundoByAtivoId(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Montante, Is.EqualTo(5000.0));
            Assert.That(result?.Nome, Is.EqualTo("Fundo A"));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("api/fundos/ativo/1")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task CreateFundo_Success_ReturnsSuccessMessage()
        {
            // Arrange
            var fundo = new FundoInvestimentoDto { Id = 1, AtivoId = 1, Nome = "Fundo A", Montante = 5000.0, TaxaJuro = 4.0 };
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.CreateFundo(fundo);

            // Assert
            Assert.That(result, Is.EqualTo("Fundo de investimento criado com sucesso."));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Post && req.RequestUri.ToString().EndsWith("api/fundos")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task UpdateFundo_Success_ReturnsSuccessMessage()
        {
            // Arrange
            var fundo = new FundoInvestimentoDto { Id = 1, AtivoId = 1, Nome = "Fundo A", Montante = 5000.0, TaxaJuro = 4.0 };
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.UpdateFundo(1, fundo);

            // Assert
            Assert.That(result, Is.EqualTo("Fundo de investimento atualizado com sucesso."));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Put && req.RequestUri.ToString().EndsWith("api/fundos/1")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task DeleteFundo_Success_ReturnsSuccessMessage()
        {
            // Arrange
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.DeleteFundo(1);

            // Assert
            Assert.That(result, Is.EqualTo("Fundo de investimento removido com sucesso."));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Delete && req.RequestUri.ToString().EndsWith("api/fundos/1")), ItExpr.IsAny<CancellationToken>());
        }
    }
}