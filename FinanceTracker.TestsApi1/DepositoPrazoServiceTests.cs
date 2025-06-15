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
    public class DepositoPrazoServiceTests
    {
        private Mock<HttpMessageHandler> _handlerMock;
        private HttpClient _httpClient;
        private DepositoPrazoService _service;

        [SetUp]
        public void SetUp()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost:5000/")
            };
            _service = new DepositoPrazoService(_httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
        }

        [Test]
        public async Task GetAllDepositos_Success_ReturnsDepositos()
        {
            // Arrange
            var depositos = new List<DepositoPrazoDto>
            {
                new DepositoPrazoDto { Id = 1, AtivoId = 1, Valor = 1000.0, Banco = "Banco A", NumeroConta = "123456", Titulares = "João Silva", TaxaJuroAnual = 2.5 },
                new DepositoPrazoDto { Id = 2, AtivoId = 2, Valor = 2000.0, Banco = "Banco B", NumeroConta = "654321", Titulares = "Maria Santos", TaxaJuroAnual = 3.0 }
            };
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(depositos), Encoding.UTF8, "application/json")
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetAllDepositos();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Count(), Is.EqualTo(2));
            Assert.That(result?.First()?.Valor, Is.EqualTo(1000.0));
            Assert.That(result?.First()?.Banco, Is.EqualTo("Banco A"));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("api/depositos")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task GetAllDepositos_Failure_ReturnsEmptyList()
        {
            // Arrange
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetAllDepositos();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetDepositoById_Success_ReturnsDeposito()
        {
            // Arrange
            var deposito = new DepositoPrazoDto { Id = 1, AtivoId = 1, Valor = 1000.0, Banco = "Banco A", NumeroConta = "123456", Titulares = "João Silva", TaxaJuroAnual = 2.5 };
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(deposito), Encoding.UTF8, "application/json")
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetDepositoById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Valor, Is.EqualTo(1000.0));
            Assert.That(result?.Banco, Is.EqualTo("Banco A"));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("api/depositos/1")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task GetDepositoById_Failure_ReturnsNull()
        {
            // Arrange
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetDepositoById(1);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetDepositoByAtivoId_Success_ReturnsDeposito()
        {
            // Arrange
            var deposito = new DepositoPrazoDto { Id = 1, AtivoId = 1, Valor = 1000.0, Banco = "Banco A", NumeroConta = "123456", Titulares = "João Silva", TaxaJuroAnual = 2.5 };
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(deposito), Encoding.UTF8, "application/json")
            };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetDepositoByAtivoId(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Valor, Is.EqualTo(1000.0));
            Assert.That(result?.Banco, Is.EqualTo("Banco A"));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("api/depositos/ativo/1")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task GetDepositoByAtivoId_Fallback_ReturnsFilteredDeposito()
        {
            // Arrange
            var depositos = new List<DepositoPrazoDto>
            {
                new DepositoPrazoDto { Id = 1, AtivoId = 1, Valor = 1000.0, Banco = "Banco A", NumeroConta = "123456", Titulares = "João Silva", TaxaJuroAnual = 2.5 },
                new DepositoPrazoDto { Id = 2, AtivoId = 2, Valor = 2000.0, Banco = "Banco B", NumeroConta = "654321", Titulares = "Maria Santos", TaxaJuroAnual = 3.0 }
            };
            var responseNotFound = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };
            var responseSuccess = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(depositos), Encoding.UTF8, "application/json")
            };

            _handlerMock
                .Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseNotFound)
                .ReturnsAsync(responseSuccess);

            // Act
            var result = await _service.GetDepositoByAtivoId(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.AtivoId, Is.EqualTo(1));
            Assert.That(result?.Banco, Is.EqualTo("Banco A"));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("api/depositos/ativo/1")), ItExpr.IsAny<CancellationToken>());
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("api/depositos")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task CreateDeposito_Success_ReturnsSuccessMessage()
        {
            // Arrange
            var deposito = new DepositoPrazoDto { Id = 1, AtivoId = 1, Valor = 1000.0, Banco = "Banco A", NumeroConta = "123456", Titulares = "João Silva", TaxaJuroAnual = 2.5 };
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.CreateDeposito(deposito);

            // Assert
            Assert.That(result, Is.EqualTo("Depósito de prazo criado com sucesso."));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Post && req.RequestUri.ToString().EndsWith("api/depositos")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task UpdateDeposito_Success_ReturnsSuccessMessage()
        {
            // Arrange
            var deposito = new DepositoPrazoDto { Id = 1, AtivoId = 1, Valor = 1000.0, Banco = "Banco A", NumeroConta = "123456", Titulares = "João Silva", TaxaJuroAnual = 2.5 };
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.UpdateDeposito(1, deposito);

            // Assert
            Assert.That(result, Is.EqualTo("Depósito de prazo atualizado com sucesso."));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Put && req.RequestUri.ToString().EndsWith("api/depositos/1")), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task DeleteDeposito_Success_ReturnsSuccessMessage()
        {
            // Arrange
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _service.DeleteDeposito(1);

            // Assert
            Assert.That(result, Is.EqualTo("Depósito de prazo removido com sucesso."));
            _handlerMock.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Delete && req.RequestUri.ToString().EndsWith("api/depositos/1")), ItExpr.IsAny<CancellationToken>());
        }
    }
}