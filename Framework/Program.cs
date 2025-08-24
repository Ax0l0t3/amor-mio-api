using System.Text.Json;
using Framework.Models;
using static Framework.Services.PrintService;

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

app.MapGet("/get-printers", async () =>
{
    var filePath = "../mockPrinters.json";
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

app.MapPost("/save-printers", async (PrintersClass data) =>
{
    try
    {
        var filePath = "../mockPrinters.json";
        var jsonContent = JsonSerializer.Serialize(data);
        await File.WriteAllTextAsync(filePath, jsonContent);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.Problem($"Unexpected behaviour {ex.Message}");
    }
});

app.MapPost("/printJson", async (HttpContext httpData) =>
{
    try
    {
        using var reader = new StreamReader(httpData.Request.Body);
        var data = await reader.ReadToEndAsync();
        PrintOverTcp_Ip("192.168.1.100", 9100, data);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.Problem($"Unexpected behaviour {ex.Message}");
    }
});

app.Run();