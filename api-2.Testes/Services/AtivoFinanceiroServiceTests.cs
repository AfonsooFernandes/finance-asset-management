using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceTracker.Models;
using FinanceTracker.Services;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace api_2.Testes.Services
{
    public class AtivoFinanceiroServiceTests
    {
        private AtivoFinanceiroService _service;
        private Mock<HttpMessageHandler> _httpMessageHandler;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>();

            var httpClient = new HttpClient(_httpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:5232/")
            };

            _service = new AtivoFinanceiroService(httpClient);
        }

        // Método utilitário para configurar mock, com filtros opcionais para URI e método HTTP
        private void SetupHttpResponse(HttpResponseMessage response, string expectedUrl = null, HttpMethod method = null)
        {
            _httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        (expectedUrl == null || req.RequestUri!.ToString().Contains(expectedUrl)) &&
                        (method == null || req.Method == method)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
        }

        [Test]
        public async Task GetAllAtivos_ShouldReturnListOfAtivos()
        {
            var expectedAtivos = new List<AtivoFinanceiroDto>
            {
                new AtivoFinanceiroDto { Id = 1, Tipo = "Ação XPTO", LucroTotalAntesImpostos = 1000m },
                new AtivoFinanceiroDto { Id = 2, Tipo = "ETF ABC", LucroTotalAntesImpostos = 1500m }
            };

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expectedAtivos)
            };

            SetupHttpResponse(response); // Sem filtro, para qualquer requisição

            var result = await _service.GetAllAtivos();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Tipo, Is.EqualTo("Ação XPTO"));
        }

        [Test]
        public async Task GetAtivoById_ShouldReturnAtivo_WhenExists()
        {
            var ativo = new AtivoFinanceiroDto { Id = 1, Tipo = "Título XP", LucroTotalAntesImpostos = 500m };

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(ativo)
            };

            SetupHttpResponse(response, "api/ativos/1");

            var result = await _service.GetAtivoById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Tipo, Is.EqualTo("Título XP"));
        }

        [Test]
        public async Task GetAtivosByUserId_ShouldReturnList()
        {
            var ativos = new List<AtivoFinanceiroDto>
            {
                new AtivoFinanceiroDto { Id = 1, Tipo = "XP", LucroTotalAntesImpostos = 300m }
            };

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(ativos)
            };

            SetupHttpResponse(response, "api/ativos/usuario/5");

            var result = await _service.GetAtivosByUserId(5);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Tipo, Is.EqualTo("XP"));
        }

        [Test]
        public async Task CreateAtivo_ShouldReturnCreatedAtivo()
        {
            var input = new AtivoFinanceiroDto { Tipo = "Novo", LucroTotalAntesImpostos = 100m };
            var returned = new AtivoFinanceiroDto { Id = 10, Tipo = "Novo", LucroTotalAntesImpostos = 100m };

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(returned)
            };

            SetupHttpResponse(response, "api/ativos", HttpMethod.Post);

            var result = await _service.CreateAtivo(input);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(10));
            Assert.That(result.Tipo, Is.EqualTo("Novo"));
        }

        [Test]
        public async Task UpdateAtivo_ShouldReturnSuccessMessage()
        {
            // Use propriedades que existem em AtivoFinanceiroDto (substituí Nome e Valor por Tipo e LucroTotalAntesImpostos)
            var input = new AtivoFinanceiroDto { Id = 1, Tipo = "Atualizado", LucroTotalAntesImpostos = 999m };

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            SetupHttpResponse(response, "api/ativos/1", HttpMethod.Put);

            var result = await _service.UpdateAtivo(1, input);

            Assert.That(result, Is.EqualTo("Ativo financeiro atualizado com sucesso."));
        }

        [Test]
        public async Task DeleteAtivo_ShouldReturnSuccessMessage()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            SetupHttpResponse(response, "api/ativos/1", HttpMethod.Delete);

            var result = await _service.DeleteAtivo(1);

            Assert.That(result, Is.EqualTo("Ativo financeiro removido com sucesso."));
        }
    }
}
