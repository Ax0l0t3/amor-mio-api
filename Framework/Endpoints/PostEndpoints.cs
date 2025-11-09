using System.Text.Json;
using Framework.Models;
using Framework.Observers;
using Framework.Publishers;
using Microsoft.AspNetCore.Mvc;
using static Framework.Constants.StringConstants;

namespace Framework.Endpoints
{
    public static class PostEndpoints
    {
        public static void MapPostEndpoints(this IEndpointRouteBuilder routes, PrinterPublisher printerPublisher, TabPublisher tabPublisher)
        {
            routes.MapPost("/post-bg-image", async ([FromForm] IFormFile file) =>
            {
                try
                {
                    if (file == null || file.Length == 0)
                    {
                        return Results.BadRequest("No file uploaded.");
                    }
                    string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), BgImageFilePath);
                    string[] thisStringList = Directory.GetFiles(BgImageFilePath);
                    foreach (var thisString in thisStringList)
                    {
                        File.Delete(thisString);
                    }
                    var filePath = Path.Combine(directoryPath, file.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Unexpected error: {ex.Message}");
                }
            }).DisableAntiforgery();

            routes.MapPost("/post-colours", async (List<string> data) =>
            {
                try
                {
                    var jsonContent = JsonSerializer.Serialize(data);
                    await File.WriteAllTextAsync(ColoursFilePath, jsonContent);
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Unexpected behaviour {ex.Message}");
                }
            });

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
                    var printersJson = await File.ReadAllTextAsync(PrintersFilePath);
                    PrintersClass previousPrintersState = JsonSerializer.Deserialize<PrintersClass>(printersJson) ?? new PrintersClass();
                    var menuJson = await File.ReadAllTextAsync(DataFilePath);
                    MenuObject currentTabs = JsonSerializer.Deserialize<MenuObject>(menuJson) ?? new MenuObject();
                    foreach (OptionTab thisTab in currentTabs.Tabs)
                    {
                        tabPublisher.Attach(new OptionTabObserver(thisTab));
                    }
                    foreach (Printer uiPrinter in data.Printers)
                    {
                        Printer? foundPrinter = previousPrintersState.Printers.Find(p => p.Id == uiPrinter.Id);
                        if (foundPrinter == null)
                        {
                            continue;
                        }

                        if (foundPrinter.Name != uiPrinter.Name)
                        {
                            tabPublisher.SetPrintersNames(foundPrinter.Name, uiPrinter.Name);
                            tabPublisher.Notify();
                        }
                    }
                    tabPublisher.DetachAll();
                    tabPublisher.ResetStates();

                    var jsonMenu = JsonSerializer.Serialize(currentTabs);

                    await File.WriteAllTextAsync(PrintersFilePath, jsonContent);
                    await File.WriteAllTextAsync(DataFilePath, jsonMenu);

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
                    Console.WriteLine("/printJson Endpoint started");
                    using var reader = new StreamReader(httpData.Request.Body);
                    var message = await reader.ReadToEndAsync();
                    Console.Write("Setting new publisher message...");
                    printerPublisher.SetPrinterService(message);
                    Console.WriteLine("\tPublisher message done");
                    Console.Write("Notifying printer observers...");
                    printerPublisher.Notify();
                    Console.WriteLine("\tObservers notified");
                    Console.WriteLine("Returning Ok");
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Unexpected behaviour {ex.Message}");
                }
            });

            routes.MapPost("/open-connection", async () =>
            {
                try
                {
                    var printersJson = await File.ReadAllTextAsync(PrintersFilePath);
                    PrintersClass thisPrinters = JsonSerializer.Deserialize<PrintersClass>(printersJson) ?? new PrintersClass();
                    Console.Write("Attaching printer observers...");
                    foreach (var thisPrinter in thisPrinters.Printers)
                    {
                        printerPublisher.Attach(new PrinterObserver(thisPrinter));
                    }
                    Console.WriteLine("\tPrinter observers attached");
                    printerPublisher.SetNewConnections();
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