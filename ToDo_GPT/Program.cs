using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using ToDo_GPT.Context;
using ToDo_GPT.Extensions;
using ToDo_GPT.Logging;
using ToDo_GPT.Repositories;
using ToDo_GPT.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Conexão com o banco de dados
string? mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseMySql(mySqlConnection,
          ServerVersion.AutoDetect(mySqlConnection)));

// Registra a configuração do logger
builder.Services.AddSingleton(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Trace // Configure o nível de log conforme necessário
});

// Registra o logger com parâmetros
builder.Services.AddSingleton<CustomerLogger>(serviceProvider =>
{
    var config = serviceProvider.GetRequiredService<CustomLoggerProviderConfiguration>();
    return new CustomerLogger("DefaultLogger", config);
});

// Registra o repositório
builder.Services.AddScoped<IUserRepository, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler(app.Services.GetRequiredService<CustomerLogger>());
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();