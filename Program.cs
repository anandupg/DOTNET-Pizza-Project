using PizzaApi.Models;
using PizzaApi.Services;

var builder = WebApplication.CreateBuilder(args);
// 1. Add this to allow the web page to talk to the API
builder.Services.AddCors(); 
// Configure MongoDB settings from configuration and register services
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
builder.Services.AddSingleton<PizzaService>();
builder.Services.AddSingleton<UserService>();

var app = builder.Build();

app.UseDefaultFiles(); // Tells .NET to look for index.html
app.UseStaticFiles();  // Tells .NET to serve files from wwwroot folder
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Endpoints backed by MongoDB service
app.MapGet("/pizzas", async (PizzaService svc) => 
{
    try
    {
        return Results.Ok(await svc.GetAsync());
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.ToString(), statusCode: 500);
    }
});

app.MapPost("/pizzas", async (PizzaService svc, Pizza pizza) =>
{
    await svc.CreateAsync(pizza);
    return Results.Created($"/pizzas/{pizza.Id}", pizza);
});

// Authentication Endpoints
app.MapPost("/login", async (UserService svc, User loginReq) =>
{
    var user = await svc.LoginAsync(loginReq.Username, loginReq.Password);
    if (user is null) return Results.Unauthorized();
    return Results.Ok(user);
});

app.MapPost("/setup", async (UserService svc) =>
{
    await svc.CreateAsync(new User { Username = "admin", Password = "admin123", Role = "Admin" });
    await svc.CreateAsync(new User { Username = "user", Password = "user123", Role = "User" });
    return Results.Ok("Users initialized");
});

app.Run();