using Store4Dev.Application.AutoMapper;
using Store4Dev.Application.Services;
using Store4Dev.Application.Services.Support;
using Store4Dev.Domain.Repositories;
using Store4Dev.Domain.Services;
using Store4Dev.Domain.Services.Support;
using Store4Dev.Data;
using Store4Dev.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper
builder.Services.AddAutoMapper(typeof(StoreProfile));

// EF
builder.Services.AddDbContext<StoreContext>();

// Services
builder.Services.AddTransient<IProductAppService, ProductAppService>();
builder.Services.AddTransient<IStockService, StockService>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Routes
app.ConfigureRoutes();

app.Run();