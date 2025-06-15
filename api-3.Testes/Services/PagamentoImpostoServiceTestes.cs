using FinanceTracker.Models;
using FinanceTracker.Services;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FinanceTracker.Tests
{
    public class PagamentoImpostoServiceTestes
    {
        private Mock<HttpMessageHandler> _handlerMock;
        private HttpClient _httpClient;
        private PagamentoImpostosService _service;

        [SetUp]
        public void Setup()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };
            _service = new PagamentoImpostosService(_httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
        }

        [Test]
        public async Task GetAllAsync_DeveRetornarListaDePagamentos()
        {
            var mockData = new List<PagamentoImpostosDto>
            {
                new PagamentoImpostosDto { Id = 1, AtivoId = 10, DataPagamento = new DateTime(2024, 5, 1), Valor = 150.75m },
                new PagamentoImpostosDto { Id = 2, AtivoId = 11, DataPagamento = new DateTime(2024, 6, 1), Valor = 200.00m }
            };

            var json = JsonSerializer.Serialize(mockData);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("api/pagamentos")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var result = await _service.GetAllAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Valor, Is.EqualTo(150.75m));
            Assert.That(result[1].AtivoId, Is.EqualTo(11));
        }

        [Test]
        public async Task GetByIdAsync_DeveRetornarPagamentoPorId()
        {
            var mockItem = new PagamentoImpostosDto
            {
                Id = 5,
                AtivoId = 20,
                DataPagamento = new DateTime(2025, 1, 15),
                Valor = 99.99m
            };

            var json = JsonSerializer.Serialize(mockItem);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("api/pagamentos/5")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var result = await _service.GetByIdAsync(5);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(5));
            Assert.That(result.AtivoId, Is.EqualTo(20));
            Assert.That(result.DataPagamento, Is.EqualTo(new DateTime(2025, 1, 15)));
            Assert.That(result.Valor, Is.EqualTo(99.99m));
        }

        [Test]
        public async Task AddAsync_DeveRetornarMensagemDeSucesso()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Pagamento adicionado com sucesso")
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var novoPagamento = new PagamentoImpostosDto
            {
                AtivoId = 99,
                DataPagamento = new DateTime(2025, 3, 10),
                Valor = 450.00m
            };

            var result = await _service.AddAsync(novoPagamento);

            Assert.That(result, Is.EqualTo("Pagamento adicionado com sucesso"));
        }

        [Test]
        public async Task UpdateAsync_DeveRetornarMensagemDeSucesso()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Pagamento atualizado com sucesso")
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var atualizado = new PagamentoImpostosDto
            {
                Id = 7,
                AtivoId = 88,
                DataPagamento = new DateTime(2025, 4, 20),
                Valor = 300.00m
            };

            var result = await _service.UpdateAsync(7, atualizado);

            Assert.That(result, Is.EqualTo("Pagamento atualizado com sucesso"));
        }

        [Test]
        public async Task DeleteAsync_DeveRetornarMensagemDeSucesso()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Pagamento removido com sucesso")
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var result = await _service.DeleteAsync(3);

            Assert.That(result, Is.EqualTo("Pagamento removido com sucesso"));
        }
    }
}
