using Microsoft.EntityFrameworkCore;
using RickGuitars.SmellyApi.Data;
using RickGuitars.SmellyApi.External;
using RickGuitars.SmellyApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<RickGuitarsDbContext>(options =>
{
    options.UseInMemoryDatabase("RickGuitarsSmellyDb");
});

builder.Services.AddScoped<FakeStripeClient>();
builder.Services.AddScoped<FakeShippingClient>();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<DiscountService>();
builder.Services.AddScoped<ShippingService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RickGuitarsDbContext>();
    SeedData.Initialize(db);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();