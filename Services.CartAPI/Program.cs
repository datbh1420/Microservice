using BackEnd.CartAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Services.CartAPI.Extensions;
using Services.CartAPI.Services;
using Services.CartAPI.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
  {
    {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
      },
      new string[] {}
    }
  });
});

//Add MassTransit RabbitMQ
builder.Services.AddMassTransitRabbitMQ(builder.Configuration);


//DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PandaDBConnectionString"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}
);

//Config SendAsync
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BackEndApiAuthenticationHttpClientHandler>();
builder.Services.AddHttpClient("Product", client =>
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"])
).AddHttpMessageHandler<BackEndApiAuthenticationHttpClientHandler>();
builder.Services.AddHttpClient("Coupon", client =>
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"])
).AddHttpMessageHandler<BackEndApiAuthenticationHttpClientHandler>();

// Add Authentication
builder.AddAppAuthentication();
builder.Services.AddAuthentication();

//DI
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<ICartService, CartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ApplyMigration();
app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}
