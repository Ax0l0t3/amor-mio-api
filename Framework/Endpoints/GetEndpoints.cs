namespace Framework.Endpoints
{
    public static class GetEndpoints
    {
        public static void MapGetEndpoints(this IEndpointRouteBuilder routes)
        {
            string applicationType = "application/json";

            routes.MapGet("/data-menu", async () =>
            {
                var filePath = "../mockData-Menu.json";
                var jsonContent = await File.ReadAllTextAsync(filePath);
                return Results.Content(jsonContent, applicationType);
            });

            routes.MapGet("/data-empty", async () =>
            {
                var filePath = "../mockData-Empty.json";
                var jsonContent = await File.ReadAllTextAsync(filePath);
                return Results.Content(jsonContent, applicationType);
            });

            routes.MapGet("/data-test-cases", async () =>
            {
                var filePath = "../mockData-TestCases.json";
                var jsonContent = await File.ReadAllTextAsync(filePath);
                return Results.Content(jsonContent, applicationType);
            });

            routes.MapGet("/get-printers", async () =>
            {
                var filePath = "../mockPrinters.json";
                var jsonContent = await File.ReadAllTextAsync(filePath);
                return Results.Content(jsonContent, applicationType);
            });
        }
    }
}