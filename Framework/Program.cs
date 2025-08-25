using Framework.Publishers;
using static Framework.Endpoints.GetEndpoints;
using static Framework.Endpoints.PostEndpoints;

PrinterPublisher printerPublisher = new PrinterPublisher();

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

app.MapGetEndpoints();
app.MapPostEndpoints(printerPublisher);

app.Run();