using Microsoft.EntityFrameworkCore;
using Nexora.Identity.Application.Features.Users.CreateUser;
using Nexora.Identity.Application.Features.Users.GetUsers;
using Nexora.Identity.Domain.Repositories;
using Nexora.Identity.Infrastructure.Persistence;
using Nexora.Identity.Infrastructure.Repositories;
using Nexora.Identity.Application.Security;
using Nexora.Identity.Infrastructure.Security;
using Nexora.Identity.Application.Features.Users.Verification.GenerateRegistrationOtp;
using Nexora.Identity.Application.Features.Users.Verification.VerifyRegistrationOtp;

var builder = WebApplication.CreateBuilder(args);

// =====================================
// Services
// =====================================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL / EF Core
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("NexoraDatabase")));

// Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<
    IUserVerificationRepository,
    UserVerificationRepository>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<CreateUserHandler>();
builder.Services.AddScoped<GetUsersHandler>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IOtpGenerator, OtpGenerator>();
builder.Services.AddScoped<GenerateRegistrationOtpHandler>();
builder.Services.AddScoped<VerifyRegistrationOtpHandler>();

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
// Temporary platform heartbeat
// =====================================

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

// =====================================
// Controllers
// =====================================

app.MapControllers();

app.Run();