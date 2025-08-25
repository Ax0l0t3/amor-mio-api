using static Framework.Constants.StringConstants;
namespace Framework.Endpoints
{
    public static class GetEndpoints
    {
        public static void MapGetEndpoints(this IEndpointRouteBuilder routes)
        {
            string applicationType = "application/json";

            routes.MapGet("/data-menu", async () =>
            {
                var jsonContent = await File.ReadAllTextAsync(DataFilePath);
                return Results.Content(jsonContent, applicationType);
            });

            routes.MapGet("/data-empty", async () =>
            {
                var jsonContent = await File.ReadAllTextAsync(EmptyDataFilePath);
                return Results.Content(jsonContent, applicationType);
            });

            routes.MapGet("/data-test-cases", async () =>
            {
                var jsonContent = await File.ReadAllTextAsync(TestDataFilePath);
                return Results.Content(jsonContent, applicationType);
            });

            routes.MapGet("/get-printers", async () =>
            {
                var jsonContent = await File.ReadAllTextAsync(PrintersFilePath);
                return Results.Content(jsonContent, applicationType);
            });
        }
    }
}