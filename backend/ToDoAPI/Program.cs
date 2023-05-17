using Microsoft.EntityFrameworkCore;
using ToDoAPI.Models;

string? dbHostString = Environment.GetEnvironmentVariable("db_host_string");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();


builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
builder.Services.AddDbContext<ToDoManagementDbContext>(options =>
{
    string? connectionString = dbHostString != null ? dbHostString : builder.Configuration["ConnectionStrings:ToDoManagementDbContextConnection"];
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthz");

app.Run();
