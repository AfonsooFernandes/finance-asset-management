using System.Threading.Tasks;
using FinanceTracker.Data;
using FinanceTracker.Models;
using FinanceTracker.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace api_2.Testes.Services
{
    public class AuthenticationServiceTests
    {
        private FinanceTrackerContext _context;
        private AuthenticationService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FinanceTrackerContext>()
                .UseInMemoryDatabase(databaseName: "FinanceTrackerTestDB")
                .Options;

            _context = new FinanceTrackerContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _service = new AuthenticationService(_context);
        }

        [TearDown]
        public void TearDown() => _context.Dispose();

        [Test]
        public async Task EmailExiste_RetornaTrue_SeEmailExistir()
        {
            var user = new Utilizador
            {
                Nome = "Teste",
                Email = "teste@exemplo.com",
                SenhaHash = _service.HashPassword("senha"),
                TipoUtilizador = "USER"
            };
            _context.Utilizadores.Add(user);
            await _context.SaveChangesAsync();

            var resultado = await _service.EmailExiste("teste@exemplo.com");
            Assert.That(resultado, Is.True);
        }

        [Test]
        public async Task EmailExiste_RetornaFalse_SeEmailNaoExistir()
        {
            var resultado = await _service.EmailExiste("naoexiste@exemplo.com");
            Assert.That(resultado, Is.False);
        }

        [Test]
        public async Task CriarUtilizador_AdicionaUsuarioEChamaSaveChanges()
        {
            var newUser = new Utilizador
            {
                Nome = "Novo",
                Email = "novo@exemplo.com",
                SenhaHash = _service.HashPassword("senha"),
                TipoUtilizador = "USER"
            };

            await _service.CriarUtilizador(newUser);
            var saved = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Email == "novo@exemplo.com");

            Assert.That(saved, Is.Not.Null);
            Assert.That(saved.Nome, Is.EqualTo("Novo"));
        }

        [Test]
        public async Task ValidarCredenciais_RetornaUsuarioSeSenhaCorreta()
        {
            var senha = "senha123";
            var user = new Utilizador
            {
                Nome = "Valido",
                Email = "user@exemplo.com",
                SenhaHash = _service.HashPassword(senha),
                TipoUtilizador = "USER"
            };
            _context.Utilizadores.Add(user);
            await _context.SaveChangesAsync();

            var val = await _service.ValidarCredenciais("user@exemplo.com", senha);
            Assert.That(val, Is.Not.Null);
            Assert.That(val.Email, Is.EqualTo("user@exemplo.com"));
        }

        [Test]
        public async Task ValidarCredenciais_RetornaNullSeUsuarioNaoEncontrado()
        {
            var val = await _service.ValidarCredenciais("ausente@exemplo.com", "senha");
            Assert.That(val, Is.Null);
        }

        [Test]
        public async Task ValidarCredenciais_RetornaNullSeSenhaIncorreta()
        {
            var user = new Utilizador
            {
                Nome = "Invalido",
                Email = "user@exemplo.com",
                SenhaHash = _service.HashPassword("senha_correta"),
                TipoUtilizador = "USER"
            };
            _context.Utilizadores.Add(user);
            await _context.SaveChangesAsync();

            var val = await _service.ValidarCredenciais("user@exemplo.com", "senha_errada");
            Assert.That(val, Is.Null);
        }

        [Test]
        public void HashPassword_RetornaHashCorreto()
        {
            var pwd = "teste123";
            var h = _service.HashPassword(pwd);

            Assert.That(h, Is.Not.Null);
            Assert.That(h.Length, Is.EqualTo(64));
        }

        [Test]
        public async Task ObterUtilizadorPorId_RetornaUsuarioCorreto()
        {
            var user1 = new Utilizador
            {
                Nome = "User1",
                Email = "user1@exemplo.com",
                SenhaHash = _service.HashPassword("senha1"),
                TipoUtilizador = "ADMIN"
            };
            var user2 = new Utilizador
            {
                Nome = "User2",
                Email = "user2@exemplo.com",
                SenhaHash = _service.HashPassword("senha2"),
                TipoUtilizador = "USER"
            };
            _context.Utilizadores.AddRange(user1, user2);
            await _context.SaveChangesAsync();

            var fetched = await _service.ObterUtilizadorPorId(user1.Id);
            Assert.That(fetched, Is.Not.Null);
            Assert.That(fetched.Email, Is.EqualTo("user1@exemplo.com"));
        }
    }
}
