using FinanceTracker.Data;
using FinanceTracker.Models;
using FinanceTracker.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAntiforgery();
builder.Services.AddDbContext<FinanceTrackerContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<AtivoFinanceiroService>();
builder.Services.AddScoped<DepositoPrazoService>();

builder.Services.AddHttpClient<AtivoFinanceiroService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5232/");
});

builder.Services.AddHttpClient<DepositoPrazoService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5232/");
});

builder.Services.AddHttpClient<FundoInvestimentoService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5232/");
});

builder.Services.AddHttpClient<ImovelArrendadoService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5232/");
});

var app = builder.Build();

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
app.UseAntiforgery();
app.UseAuthorization();

app.MapGet("/", async context => {
    context.Response.Redirect("/login");
    await Task.CompletedTask;
});

app.MapRazorPages();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FinanceTrackerContext>();
    var authService = scope.ServiceProvider.GetRequiredService<AuthenticationService>();

    context.Database.EnsureCreated();

    var adminExists = context.Utilizadores.Any(u => u.Email == "admin@ipvc.pt" && u.TipoUtilizador == "ADMIN");
    if (!adminExists)
    {
        var adminUser = new Utilizador
        {
            Nome = "Admin",
            Email = "admin@ipvc.pt",
            SenhaHash = authService.HashPassword("Password123"),
            TipoUtilizador = "ADMIN"
        };
        context.Utilizadores.Add(adminUser);
        context.SaveChanges();
    }
}

app.Run();