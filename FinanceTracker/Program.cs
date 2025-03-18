using FinanceTracker.Components;
using FinanceTracker.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinanceTrackerContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FinanceTrackerContext>();

    var novoUtilizador = new Utilizador
    {
        nome = "Afonso Fernandes",
        email = "afonsofernandes@ipvc.pt",
        senha_hash = "123456",
        tipo_utilizador = "USER"
    };
    
    context.Utilizadores.Add(novoUtilizador);
    context.SaveChanges();
    Console.WriteLine("O utilizador foi criado com sucesso!");
}
app.Run();