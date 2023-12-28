using Microsoft.EntityFrameworkCore;
using SharedModels;
using TaskTrackerApi.Data;
using TaskTrackerApi.Infrastructure;
using TaskTrackerApi.Models;
using static TaskTrackerApi.Models.TaskConverter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string rabbitmqConn = "host=rabbitmq";

builder.Services.AddSingleton<IMessagePublisher>(new MessagePublisher(rabbitmqConn));


builder.Services.AddDbContext<TaskApiContext>(opt => opt.UseInMemoryDatabase("TaskDb"));

// Register repositories for dependency injection
builder.Services.AddScoped<IRepository<MyTask>, TaskRepository>();

// Register database initializer for dependency injection
builder.Services.AddTransient<IDbInitializer, DbInitializer>();

// Register ProductConverter for dependency injection
builder.Services.AddSingleton<IConverter<MyTask, MyTaskDto>, ProductConverter>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(config => config
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
);

// Initialize the database.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetService<TaskApiContext>();
    var dbInitializer = services.GetService<IDbInitializer>();
    dbInitializer.Initialize(dbContext);
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
