using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using Microsoft.Extensions.Configuration;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetValue<string>("CacheSettings:ConnectionStrings");
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
    (o => o.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]));

builder.Services.AddScoped<DiscountGrpcService>();

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
