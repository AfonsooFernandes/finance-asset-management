using FinanceTracker.Models;
using FinanceTracker.Services;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FinanceTracker.Tests
{
    public class JurosMensaisFundoServiceTestes
    {
        private Mock<HttpMessageHandler> _handlerMock;
        private HttpClient _httpClient;
        private JurosMensaisFundoService _service;

        [SetUp]
        public void Setup()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new System.Uri("http://localhost/")
            };
            _service = new JurosMensaisFundoService(_httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
        }

        [Test]
        public async Task GetAllAsync_DeveRetornarListaDeJuros()
        {
            var mockData = new List<JurosMensaisFundoDto>
            {
                new JurosMensaisFundoDto { Id = 1, FundoId = 100, Mes = 1, Ano = 2024, Taxa = 0.05 },
                new JurosMensaisFundoDto { Id = 2, FundoId = 100, Mes = 2, Ano = 2024, Taxa = 0.07 }
            };
            var json = JsonSerializer.Serialize(mockData);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("api/juros")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var result = await _service.GetAllAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Mes, Is.EqualTo(1));
            Assert.That(result[1].Taxa, Is.EqualTo(0.07).Within(0.0001));
        }

        [Test]
        public async Task GetByIdAsync_DeveRetornarJurosPorId()
        {
            var mockItem = new JurosMensaisFundoDto { Id = 10, FundoId = 200, Mes = 3, Ano = 2025, Taxa = 0.08 };
            var json = JsonSerializer.Serialize(mockItem);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("api/juros/10")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var result = await _service.GetByIdAsync(10);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(10));
            Assert.That(result.FundoId, Is.EqualTo(200));
            Assert.That(result.Mes, Is.EqualTo(3));
            Assert.That(result.Ano, Is.EqualTo(2025));
            Assert.That(result.Taxa, Is.EqualTo(0.08).Within(0.0001));
        }

        [Test]
        public async Task AddAsync_DeveRetornarMensagemDeSucesso()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Adicionado com sucesso")
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var novoJuro = new JurosMensaisFundoDto { FundoId = 100, Mes = 6, Ano = 2025, Taxa = 0.12 };
            var result = await _service.AddAsync(novoJuro);

            Assert.That(result, Is.EqualTo("Adicionado com sucesso"));
        }

        [Test]
        public async Task UpdateAsync_DeveRetornarMensagemDeSucesso()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Atualizado com sucesso")
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var atualizado = new JurosMensaisFundoDto { Id = 1, FundoId = 100, Mes = 7, Ano = 2025, Taxa = 0.14 };
            var result = await _service.UpdateAsync(1, atualizado);

            Assert.That(result, Is.EqualTo("Atualizado com sucesso"));
        }

        [Test]
        public async Task DeleteAsync_DeveRetornarMensagemDeSucesso()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Removido com sucesso")
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var result = await _service.DeleteAsync(1);

            Assert.That(result, Is.EqualTo("Removido com sucesso"));
        }
    }
}
