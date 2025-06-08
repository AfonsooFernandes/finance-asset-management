using FinanceTracker.Data;
using FinanceTracker.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToPage("/Login");
    options.Conventions.AllowAnonymousToPage("/Register");
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAntiforgery();

builder.Services.AddDbContext<FinanceTrackerContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<AtivoFinanceiroService>();
builder.Services.AddScoped<DepositoPrazoService>();
builder.Services.AddScoped<FundoInvestimentoService>();
builder.Services.AddScoped<ImovelArrendadoService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Login";
        options.AccessDeniedPath = "/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

var baseAddress = new Uri("https://localhost:5232/");
builder.Services.AddHttpClient<AtivoFinanceiroService>(client => client.BaseAddress = baseAddress);
builder.Services.AddHttpClient<DepositoPrazoService>(client => client.BaseAddress = baseAddress);
builder.Services.AddHttpClient<FundoInvestimentoService>(client => client.BaseAddress = baseAddress);
builder.Services.AddHttpClient<ImovelArrendadoService>(client => client.BaseAddress = baseAddress);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapGet("/", context =>
{
    context.Response.Redirect("/Login");
    return Task.CompletedTask;
});

app.MapRazorPages();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FinanceTrackerContext>();
    var authService = scope.ServiceProvider.GetRequiredService<AuthService>();

    context.Database.EnsureCreated();

    var adminExists = context.Utilizadores.Any(u => u.Email == "admin@ipvc.pt" && u.TipoUtilizador == "ADMIN");
    if (!adminExists)
    {
        var adminUser = new FinanceTracker.Models.Utilizador
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