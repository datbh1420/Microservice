using Mango.Services.EmailAPI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Services.EmailAPI.Data;
using Services.EmailAPI.Extensions;
var builder = WebApplication.CreateBuilder(args);


//Add Serilog Configuration
Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.ClearProviders().AddSerilog();
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PandaDBConnectionString"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}
);

//Add MassTransit RabbitMQ
builder.Services.AddMassTransitRabbitMQ(builder.Configuration);
//Add MediatR
builder.Services.AddMediatR();

builder.Services.AddScoped<IEmailService, EmailService>();

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