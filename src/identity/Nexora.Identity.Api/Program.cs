var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// NEXORA Home Endpoint
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

app.Run();