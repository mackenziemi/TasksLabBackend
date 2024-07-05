using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using TasksLab.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TasksLabDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.MapGet("/test", () => TypedResults.Ok("Testing 1, 2, 3,...."))
    .WithName("Test")
    .WithOpenApi(x => new OpenApiOperation(x)
     {
         Summary = "Test Query",
         Description = "Returns a simple message .",
         Tags = new List<OpenApiTag> { new() { Name = "Tasks Lab API" } }
     });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
