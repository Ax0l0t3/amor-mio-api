var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "DevelopmentCors",
    policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("DevelopmentCors");

app.MapGet("/data-menu", async () =>
{
    var filePath = "../mockData-Menu.json";
    var jsonContent = await File.ReadAllTextAsync(filePath);
    return Results.Content(jsonContent, "application/json");
});

app.MapGet("/data-empty", async () =>
{
    var filePath = "../mockData-Empty.json";
    var jsonContent = await File.ReadAllTextAsync(filePath);
    return Results.Content(jsonContent, "application/json");
});

app.MapGet("/data-test-cases", async () =>
{
    var filePath = "../mockData-TestCases.json";
    var jsonContent = await File.ReadAllTextAsync(filePath);
    return Results.Content(jsonContent, "application/json");
});

app.Run();
