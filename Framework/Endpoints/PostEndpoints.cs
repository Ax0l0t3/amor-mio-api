using System.Text.Json;
using Framework.Models;
using Framework.Observers;
using Framework.Publishers;
using static Framework.Constants.StringConstants;

namespace Framework.Endpoints
{
    public static class PostEndpoints
    {
        public static void MapPostEndpoints(this IEndpointRouteBuilder routes, PrinterPublisher printerPublisher)
        {
            routes.MapPost("/post-data-empty", async (MenuObject data) =>
            {
                try
                {
                    var jsonContent = JsonSerializer.Serialize(data);
                    await File.WriteAllTextAsync(EmptyDataFilePath, jsonContent);
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Unexpected behaviour {ex.Message}");
                }
            });

            routes.MapPost("/post-data-menu", async (MenuObject data) =>
            {
                try
                {
                    var jsonContent = JsonSerializer.Serialize(data);
                    await File.WriteAllTextAsync(DataFilePath, jsonContent);
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Unexpected behaviour {ex.Message}");
                }
            });

            routes.MapPost("/save-printers", async (PrintersClass data) =>
            {
                try
                {
                    var jsonContent = JsonSerializer.Serialize(data);
                    await File.WriteAllTextAsync(PrintersFilePath, jsonContent);
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Unexpected behaviour {ex.Message}");
                }
            });

            routes.MapPost("/printJson", async (HttpContext httpData) =>
            {
                try
                {
                    using var reader = new StreamReader(httpData.Request.Body);
                    var message = await reader.ReadToEndAsync();
                    var printersJson = await File.ReadAllTextAsync(PrintersFilePath);
                    PrintersClass thisPrinters = JsonSerializer.Deserialize<PrintersClass>(printersJson);
                    List<PrinterObserver> observers = new List<PrinterObserver>();
                    foreach (var thisPrinter in thisPrinters.Printers)
                    {
                        printerPublisher.Attach(new PrinterObserver(thisPrinter));
                    }
                    printerPublisher.SetPrinterService(message);
                    printerPublisher.Notify();
                    printerPublisher.DetachAll();
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Unexpected behaviour {ex.Message}");
                }
            });
        }
    }
}