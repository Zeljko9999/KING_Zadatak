using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ProductAPI.Configuration;
using ProductAPI.Logic;
using ProductAPI.Logic.Interfaces;
using ProductAPI.Repositories;
using ProductAPI.Repositories.Interfaces;
//using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var defaultCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

// Add services to the container.

//To include database
//builder.Services.AddDbContext<ProductAPIContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.Configure<ApiConfiguration>(builder.Configuration.GetSection("DataSources:Api"));


builder.Services.AddControllers();

builder.Services.AddHttpClient<IProductRepository, ProductRepository>((provider, client) =>
{
    var apiConfig = provider.GetRequiredService<IOptions<ApiConfiguration>>().Value;
    client.BaseAddress = new Uri(apiConfig.BaseUrl);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductLogic, ProductLogic>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
