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
using Nexora.Identity.Application.Features.Auth.Login;
using Nexora.Identity.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// =====================================
// Services
// =====================================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "bearer",
        new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description =
                "Enter the JWT access token returned by POST /api/auth/login."
        });

    options.AddSecurityRequirement(
        document => new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(
                "bearer",
                document)] = []
        });
});

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
builder.Services.AddScoped<IOtpGenerator, OtpGenerator>();
builder.Services.AddScoped<GenerateRegistrationOtpHandler>();
builder.Services.AddScoped<VerifyRegistrationOtpHandler>();
builder.Services.AddScoped<LoginHandler>();

builder.Services
    .AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetSection(JwtSettings.SectionName))
    .Validate(
        settings => !string.IsNullOrWhiteSpace(settings.Issuer),
        "JWT issuer is required.")
    .Validate(
        settings => !string.IsNullOrWhiteSpace(settings.Audience),
        "JWT audience is required.")
    .Validate(
        settings => !string.IsNullOrWhiteSpace(settings.SecretKey)
                    && settings.SecretKey.Length >= 32,
        "JWT secret key must contain at least 32 characters.")
    .Validate(
        settings => settings.ExpirationMinutes > 0,
        "JWT expiration must be greater than zero.")
    .ValidateOnStart();

builder.Services.AddScoped<
    IJwtTokenGenerator,
    JwtTokenGenerator>();

//Tokens starts


var jwtSettings = builder.Configuration
    .GetSection(JwtSettings.SectionName)
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException(
        "JWT configuration is missing.");


var signingKey = new SymmetricSecurityKey(
    Convert.FromBase64String(jwtSettings.SecretKey));

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                NameClaimType = ClaimTypes.NameIdentifier,
                RoleClaimType = ClaimTypes.Role
            };
    });
//Tokens ends

builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();
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