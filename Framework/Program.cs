using System.Text.Json;
using Framework.Models;

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

app.MapPost("/post-data-empty", async (MenuObject data) =>
{
    try
    {
        var filePath = "../mockData-Empty.json";
        var jsonContent = JsonSerializer.Serialize(data);
        await File.WriteAllTextAsync(filePath, jsonContent);
        return Results.Ok();
    }
    catch (Exception ex)
    {

        return Results.Problem($"Unexpected behaviour {ex.Message}");
    }
});

app.MapPost("/post-data-menu", async (MenuObject data) =>
{
    try
    {
        var filePath = "../mockData-Menu.json";
        var jsonContent = JsonSerializer.Serialize(data);
        await File.WriteAllTextAsync(filePath, jsonContent);
        return Results.Ok();
    }
    catch (Exception ex)
    {

        return Results.Problem($"Unexpected behaviour {ex.Message}");
    }
});

app.MapPost("/printJson", (object data) =>
{
    try
    {
        var jsonContent = JsonSerializer.Serialize(data);
        Console.WriteLine(jsonContent);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.Problem($"Unexpected behaviour {ex.Message}");
    }
});

app.Run();
