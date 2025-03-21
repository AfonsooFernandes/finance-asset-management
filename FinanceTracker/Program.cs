using FinanceTracker.Data;
using Microsoft.EntityFrameworkCore;
using FinanceTracker.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adicionando serviços necessários
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FinanceTrackerContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configurações para diferentes ambientes
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Redirecionar a rota raiz para a página de login
app.MapGet("/", async context => {
    context.Response.Redirect("/login");
    await Task.CompletedTask;
});

app.MapRazorPages();
app.MapControllers();

// Verifica e cria, se necessário, um usuário admin padrão
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FinanceTrackerContext>();
    context.Database.EnsureCreated();

    var adminExists = context.Utilizadores.Any(u => u.Email == "admin@ipvc.pt" && u.TipoUtilizador == "ADMIN");
    if (!adminExists)
    {
        var adminUser = new Utilizador
        {
            Nome = "Admin",
            Email = "admin@ipvc.pt",
            SenhaHash = HashPassword("Password123"),
            TipoUtilizador = "ADMIN"
        };
        context.Utilizadores.Add(adminUser);
        context.SaveChanges();
    }
}

app.Run();

// Função para criar hash da senha
string HashPassword(string password)
{
    using (var sha256 = SHA256.Create())
    {
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
    }
}