var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/data-menu", async () => {
    var filePath = "mockData-Menu.json";
    var jsonContent = await File.ReadAllTextAsync(filePath);
    return Results.Content(jsonContent, "application/json");
});

app.MapGet("/data-empty", async () => {
    var filePath = "mockData-Empty.json";
    var jsonContent = await File.ReadAllTextAsync(filePath);
    return Results.Content(jsonContent, "application/json");
});

app.MapGet("/data-test-cases", async () => {
    var filePath = "mockData-TestCases.json";
    var jsonContent = await File.ReadAllTextAsync(filePath);
    return Results.Content(jsonContent, "application/json");
});

app.Run();
