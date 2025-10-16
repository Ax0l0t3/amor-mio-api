using static Framework.Constants.StringConstants;
namespace Framework.Endpoints
{
    public static class GetEndpoints
    {
        public static void MapGetEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/bg-image", () =>
            {
                try
                {
                    string pathToFile = Path.Combine(Directory.GetCurrentDirectory(), BgImageFilePath);
                    string[] files = Directory.GetFiles(pathToFile);
                    return Results.File(files[0]);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Unexpected image file {ex.Message}");
                }
            });

            routes.MapGet("/data-menu", async () =>
            {
                var jsonContent = await File.ReadAllTextAsync(DataFilePath);
                return Results.Content(jsonContent, HttpApplicationType);
            });

            routes.MapGet("/data-empty", async () =>
            {
                var jsonContent = await File.ReadAllTextAsync(EmptyDataFilePath);
                return Results.Content(jsonContent, HttpApplicationType);
            });

            routes.MapGet("/data-test-cases", async () =>
            {
                var jsonContent = await File.ReadAllTextAsync(TestDataFilePath);
                return Results.Content(jsonContent, HttpApplicationType);
            });

            routes.MapGet("/get-colours", async () =>
            {
                var jsonContent = await File.ReadAllTextAsync(ColoursFilePath);
                return Results.Content(jsonContent, HttpApplicationType);
            });

            routes.MapGet("/get-printers", async () =>
            {
                var jsonContent = await File.ReadAllTextAsync(PrintersFilePath);
                return Results.Content(jsonContent, HttpApplicationType);
            });
        }
    }
}