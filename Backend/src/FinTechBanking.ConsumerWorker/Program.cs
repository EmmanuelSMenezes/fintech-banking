using FinTechBanking.ConsumerWorker;
using FinTechBanking.Core.Interfaces;
using FinTechBanking.Data.Repositories;
using FinTechBanking.Services.Auth;
using FinTechBanking.Services.Messaging;
using FinTechBanking.Banking.Hub;
using FinTechBanking.Banking.Services;
using FinTechBanking.Workers;
using FinTechBanking.Workers.Consumers;

var builder = Host.CreateApplicationBuilder(args);

// Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var rabbitMqConnectionString = builder.Configuration.GetConnectionString("RabbitMq");

// Repositories
builder.Services.AddScoped<IUserRepository>(sp => new UserRepository(connectionString!));
builder.Services.AddScoped<IAccountRepository>(sp => new AccountRepository(connectionString!));
builder.Services.AddScoped<ITransactionRepository>(sp => new TransactionRepository(connectionString!));

// Services
builder.Services.AddSingleton<IMessageBroker>(sp => new RabbitMqBroker(rabbitMqConnectionString!));

// Banking
var sicoobClientId = builder.Configuration["Banking:Sicoob:ClientId"];
var sicoobAccessToken = builder.Configuration["Banking:Sicoob:AccessToken"];
var sicoobApiUrl = builder.Configuration["Banking:Sicoob:ApiUrl"];
var bankServices = new Dictionary<string, SicoobBankService>
{
    { "001", new SicoobBankService(sicoobClientId!, sicoobAccessToken!, sicoobApiUrl!) }
};
builder.Services.AddSingleton<IBankingHub>(sp => new BankingHub(bankServices));

// Consumers
builder.Services.AddScoped<PixRequestConsumer>();
builder.Services.AddScoped<WithdrawalRequestConsumer>();
builder.Services.AddScoped<WebhookEventConsumer>();
builder.Services.AddScoped<ConsumerHost>();

// Hosted Service
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
