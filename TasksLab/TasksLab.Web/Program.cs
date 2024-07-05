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

app.MapGet("/tasks", async (TasksLabDbContext dbContext) =>
{
    var tasks = await dbContext.Tasks.ToListAsync();
    return tasks;
})
    .WithName("Get All Tasks")
    .WithOpenApi(x => new OpenApiOperation(x)
    {
        Summary = "Get All Tasks",
        Description = "Returns all tasks in the database.",
        Tags = new List<OpenApiTag> { new() { Name = "Tasks Lab API" } }
    });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
