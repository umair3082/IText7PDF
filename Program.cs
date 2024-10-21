using iText.Layout;
using IText7PdfPOC.Reports;
using IText8PdfPOC.Reports;
using ScottPlot.Colormaps;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;
using IText7PdfPOC;
using IText7PdfPOC.Reports.QuestPDF.InvoiceModels;
using IText7PdfPOC.Reports.QuestPDF;
using QuestPDF.Fluent;
using System.IO;
using System.Collections;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

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
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
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
    var memoryStream = GenerateReport.GenerateSamplePDF();
    //var memoryStream = GposLookupExample.ManipulatePdf();
    context.Response.ContentType = "application/pdf";
    context.Response.Headers.Add("Content-Disposition", $"inline; filename={guid}.pdf");
    // Write the MemoryStream to the response body
    memoryStream.Position = 0; // Reset the position to the beginning of the stream
    await memoryStream.CopyToAsync(context.Response.Body);

}));

app.MapGet("/newsletter/download", (Delegate)(async (HttpContext context) =>
{
    var guid = Guid.NewGuid().ToString();
    var model = InvoiceDocumentDataSource.GetInvoiceDetails();
    var document = new InvoiceDocument(model);
    var pdf = document.GeneratePdf();
    MemoryStream memoryStream = new MemoryStream(pdf);
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
app.MapPost("/validatecustomer", async (Customer customer, IValidator<Customer> validator) =>
{
    var validationResult = await validator.ValidateAsync(customer);
    if (!validationResult.IsValid)
    {
        var validate = Results.ValidationProblem(validationResult.ToDictionary());
        return validate;
    }
    //var validationResult = await validator.ValidateAsync(customer);
    //if (!validationResult.IsValid)
    //{
    //    var errors = validationResult.Errors
    //        .GroupBy(e => e.PropertyName)
    //        .ToDictionary(
    //            g => g.Key,
    //            g => g.Select(x => x.ErrorMessage).ToArray()
    //        );
    //    return Results.ValidationProblem(errors);
    //}
    return Results.Ok(customer);
}).WithName("validatecustomer");
app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
