using iText.Layout;
using IText8PdfPOC.Reports;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using IText8PdfPOC.Helpers;
using System.IO;
using IText8PdfPOC.Graphs;
using iText.Kernel.Colors;
using iText.Layout.Properties;
using IText8PdfPOC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/generatepdf", async (HttpContext context) =>
{
    try
    {
        var guid = Guid.NewGuid().ToString();

        // Generate the PDF as a MemoryStream
        using (MemoryStream ms = GenerateReport.GenerateSamplePDF())
        {
            //// Set the content type and headers for PDF
            //context.Response.ContentType = "application/pdf";
            //context.Response.Headers.Add("Content-Disposition", $"inline; filename={guid}.pdf");

            //// Return the PDF file directly
            //return Results.File(ms.ToArray(), "application/pdf", $"{guid}.pdf");
            context.Response.ContentType = "application/pdf";
            context.Response.Headers.Add("Content-Disposition", "inline; filename=sample.pdf");
            await context.Response.Body.WriteAsync(ms.ToArray());

        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error generating PDF: {ex.Message}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An error occurred while generating the PDF.");
    }
    return Results.Ok();
});
app.MapGet("/generateTestPDF",(Delegate)(async (HttpContext context) =>
{
    var guid = Guid.NewGuid().ToString();   
    var memoryStream= GenerateReport.GenerateSamplePDF();
    context.Response.ContentType = "application/pdf";
    context.Response.Headers.Add("Content-Disposition", $"inline; filename={guid}.pdf");
    // Write the MemoryStream to the response body
    memoryStream.Position = 0; // Reset the position to the beginning of the stream
    await memoryStream.CopyToAsync(context.Response.Body);

}));

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
