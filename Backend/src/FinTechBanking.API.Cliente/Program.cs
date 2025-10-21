using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.Data;
using FinTechBanking.Data.Repositories;
using FinTechBanking.Services.Messaging;
using FinTechBanking.Banking.Hub;
using FinTechBanking.Banking.Services;

// Configurar Dapper
DapperConfiguration.Configure();

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "FinTech Banking API - Cliente",
        Version = "v1",
        Description = "API Pública para Clientes - Transações PIX, Saques e Consultas",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "FinTech Banking",
            Email = "support@fintech.com"
        }
    });

    // Adicionar suporte a JWT no Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var rabbitMqConnectionString = builder.Configuration.GetConnectionString("RabbitMq")
    ?? builder.Configuration["RabbitMq:ConnectionString"];

// Register Data Services
builder.Services.AddScoped<IUserRepository>(sp => new UserRepository(connectionString));
builder.Services.AddScoped<ITransactionRepository>(sp => new TransactionRepository(connectionString));
builder.Services.AddScoped<IAccountRepository>(sp => new AccountRepository(connectionString));

// Register Message Broker
builder.Services.AddSingleton<IMessageBroker>(sp => new RabbitMqBroker(rabbitMqConnectionString));

// Register Banking Services
var sicoobClientId = builder.Configuration["Banking:Sicoob:ClientId"];
var sicoobAccessToken = builder.Configuration["Banking:Sicoob:AccessToken"];
var sicoobApiUrl = builder.Configuration["Banking:Sicoob:ApiUrl"];
var bankServices = new Dictionary<string, SicoobBankService>
{
    { "001", new SicoobBankService(sicoobClientId, sicoobAccessToken, sicoobApiUrl) }
};
builder.Services.AddSingleton<IBankingHub>(sp => new BankingHub(bankServices));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["SecretKey"])),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinTech Banking API - Cliente v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
