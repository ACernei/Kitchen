using Kitchen.Models;
using Kitchen.Services;
using Microsoft.Extensions.Logging.Console;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<KitchenManager>();
builder.Services.AddHostedService<KitchenManager>(provider => provider.GetService<KitchenManager>());

builder.Services.AddHttpClient<IOrderService, OrderService>(httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7060/");
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // allow self signed certificates
    ClientCertificateOptions = ClientCertificateOption.Manual,
    ServerCertificateCustomValidationCallback = (_, _, _, _) => true
});

builder.Services.AddLogging(c => c.AddSimpleConsole(options =>
{
    options.TimestampFormat = "[HH:mm:ss.ffff] ";
    options.SingleLine = true;
    options.ColorBehavior = LoggerColorBehavior.Disabled;
}));

// Read appsettings configuration
builder.Services.Configure<KitchenOptions>(
    builder.Configuration.GetSection(KitchenOptions.Kitchen));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
