using System.Text.Json;
using Framework.Models;
using Framework.Observers;
using Framework.Publishers;
using static Framework.Constants.StringConstants;

namespace Framework.Endpoints
{
    public static class PostEndpoints
    {
        public static void MapPostEndpoints(this IEndpointRouteBuilder routes, PrinterPublisher printerPublisher, TabPublisher tabPublisher)
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
                    using var reader = new StreamReader(httpData.Request.Body);
                    var message = await reader.ReadToEndAsync();
                    var printersJson = await File.ReadAllTextAsync(PrintersFilePath);
                    PrintersClass thisPrinters = JsonSerializer.Deserialize<PrintersClass>(printersJson) ?? new PrintersClass();
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