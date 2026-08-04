using Nexora.Identity.Application.Features.Users.CreateUser;
using Nexora.Identity.Domain.Repositories;
using Nexora.Identity.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// =====================================
// Services
// =====================================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<CreateUserHandler>();

var app = builder.Build();

// =====================================
// Middleware
// =====================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// =====================================
// Endpoints
// =====================================

// Temporary platform health endpoint
app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        Name = "NEXORA",
        Version = "1.0.0",
        Status = "Running",
        Message = "Welcome to the NEXORA Platform!"
    });
});

// API Controllers
app.MapControllers();

app.Run();