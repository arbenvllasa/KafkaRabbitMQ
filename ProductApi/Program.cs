using Confluent.Kafka;
using ProductApi.ProductServices;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebContextConnection") ?? throw new InvalidOperationException("Gabim Connection String"));


});


var config = new ProducerConfig
{
    BootstrapServers = "localhost:9092"
};

builder.Services.AddSingleton<IProducer<Null,string>>
    (x=>new ProducerBuilder<Null,string>(config).Build());



builder.Services.AddScoped<IProductService, ProductService>();


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
