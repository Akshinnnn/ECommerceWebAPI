using Data;
using Logic;
using Presentation;
using Presentation.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.

builder.Services.DataServices(Configuration);
builder.Services.LogicServices(Configuration);
builder.Services.PresentationServices();

//Serilog configuration
builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.MSSqlServer(
                connectionString: Configuration.GetConnectionString("Default"),
                tableName: "SerilogLogs",
                autoCreateSqlTable: true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
