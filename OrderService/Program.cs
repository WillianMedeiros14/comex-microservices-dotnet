using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrderService.Data;
using OrderService.ItemServiceHttpClient;
using OrderService.Repository;
using StockService.ItemServiceHttpClient;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OrderServiceConnection");

builder.Services.AddDbContext<OrderContext>(opts =>
    opts.UseLazyLoadingProxies().UseNpgsql(connectionString));


// builder.Services.AddSingleton<IRabbitMqClient, RabbitMqClient>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddHttpClient<IStockServiceHttpClient, StockServiceHttpClient>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderService", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () =>
{
    return "Executando";
})
.WithName("/")
.WithOpenApi();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program;