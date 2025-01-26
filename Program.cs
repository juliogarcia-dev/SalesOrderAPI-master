using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SalesOrderAPI.Services;
using dotenv.net;

var builder = WebApplication.CreateBuilder(args);

// 1. Adicionar serviços ao container
builder.Services.AddControllers(); // Habilitar suporte para controllers
builder.Services.AddEndpointsApiExplorer(); // Configurar documentação para APIs
builder.Services.AddSwaggerGen(); // Adicionar Swagger para documentação
builder.Configuration.AddEnvironmentVariables();  // Para carregar variáveis de ambiente
DotEnv.Load();

// 2. Configurar Entity Framework Core com PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // Usa a string de conexão no appsettings.json

// 3. Configurar autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"] ?? "chave-secreta-padrao"))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyFrontend", builder =>
        builder.WithOrigins($"{Environment.GetEnvironmentVariable("APP_URL")}") // Domínio do frontend
               .AllowAnyHeader()
               .AllowAnyMethod());
});

// Registrar o ItemsService no container de DI
builder.Services.AddScoped<IItemService, ItemsService>();
builder.Services.AddScoped<ISalesOrderService, SalesOrdersService>();

var app = builder.Build();
var IsDevelopment = app.Environment.IsDevelopment();

// 4. Configuração do pipeline de requisição HTTP
if (IsDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Adicionar middleware de autenticação
app.UseAuthorization();  // Adicionar middleware de autorização

app.MapControllers(); // Mapear os endpoints para os controllers

app.UseCors("AllowMyFrontend");

if (IsDevelopment)
{
    app.Run($"http://localhost:5033");
}
else
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
    app.Run($"http://0.0.0.0:{port}"); // Configuração da porta
}