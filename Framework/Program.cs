using Framework.Publishers;
using static Framework.Endpoints.GetEndpoints;
using static Framework.Endpoints.PostEndpoints;

PrinterPublisher printerPublisher = new PrinterPublisher();
TabPublisher tabPublisher = new TabPublisher();

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
builder.WebHost.UseUrls("http://0.0.0.0:5000");
var app = builder.Build();

app.UseCors("DevelopmentCors");
// app.UseStaticFiles();
// app.UseDefaultFiles();
// app.MapFallbackToFile("index.html");
app.MapGetEndpoints();
app.MapPostEndpoints(printerPublisher, tabPublisher);

app.Run();