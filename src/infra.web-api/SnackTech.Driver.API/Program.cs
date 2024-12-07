using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SnackTech.Common.Dto;
using SnackTech.Driver.API.Configuration;
using SnackTech.Driver.API.Configuration.HealthChecks;
using SnackTech.Driver.DataBase;
using SnackTech.Driver.DataBase.Context;
using SnackTech.Driver.MercadoPago;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MercadoPagoOptions>(builder.Configuration.GetSection("MercadoPagoOptions"));
// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddAdapterDatabaseRepositories();
builder.Services.AddMercadoPagoService();
builder.Services.AddDomainControllers();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.DescribeAllParametersInCamelCase();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SnackTech", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

string dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

if (string.IsNullOrEmpty(dbConnectionString))
{
    throw new InvalidOperationException(
        "Could not find a connection string named 'DefaultConnection'.");
}

builder.Services.AddDbContext<RepositoryDbContext>(options =>
    options.UseSqlServer(dbConnectionString));

builder.Services.AddHealthChecks()
    .ConfigureSQLHealthCheck();

var app = builder.Build();

using var scope = app.Services.CreateScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<RepositoryDbContext>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SnackTech API v1");
});

//app.UseHttpsRedirection();
app.UseCustomHealthChecks();
app.UseAuthorization();

// Redirecionamento da URL raiz para /swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.MapControllers();

app.Run();
