using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SharedModels;
using System.Reflection;
using TaskTrackerApi.Models;
using UserApi.Data;
using UserApi.Infrastructure;
using UserApi.Models;
using static TaskTrackerApi.Models.UserConverter;

var builder = WebApplication.CreateBuilder(args);

string rabbitmqConn = "host=rabbitmq";

// Add services to the container.

builder.Services.AddDbContext<UserApiContext>(opt => opt.UseInMemoryDatabase("UserDb"));

// Register repositories for dependency injection
builder.Services.AddScoped<IRepository<MyUser>, UserRepository>();

// Register database initializer for dependency injection
builder.Services.AddTransient<IDbInitializer, DbInitializer>();

// Register ProductConverter for dependency injection
builder.Services.AddSingleton<IConverter<MyUser, MyUserDto>, UserConverter>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "UserApi",
            Description = "This api handles all users",
        });
        // Use the existing XML file without modifying it during build
        var xmlFileName = "UserApi.xml";
        var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
        c.IncludeXmlComments(xmlFilePath);
        c.EnableAnnotations();

    });


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserApi V1");
        // Uncomment the line below if you want to enable Swagger UI at the root URL
        // c.RoutePrefix = string.Empty;
    });
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
    var dbContext = services.GetService<UserApiContext>();
    var dbInitializer = services.GetService<IDbInitializer>();
    dbInitializer.Initialize(dbContext);
}

// Create a message listener in a separate thread.
Task.Factory.StartNew(() =>
    new MessageListener(app.Services, rabbitmqConn).Start());

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
