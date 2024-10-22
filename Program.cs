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
using IText7PdfPOC.LangTranslator;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Newtonsoft.Json;

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

app.MapGet("/Translate", async () =>
{
    ////[FromBody]  List<string> texts
    //// var guid = Guid.NewGuid().ToString();
    //// var translation = await TranslateLanguage.TranslateLanguageMethod(text, "en","ja");
    ////return Results.Ok(translation);
    var stopWatch = new System.Diagnostics.Stopwatch();
    stopWatch.Start();
    var texts = new List<string> { "Hello",
    "World",
    "How are you?",
    "What's your name?",
    "I'm fine, thank you",
    "Good morning",
    "Good afternoon",
    "Good evening",
    "Goodbye",
    "See you later",
    "Have a nice day",
    "I love you",
    "I miss you",
    "You're welcome",
    "Excuse me",
    "Sorry",
    "Thank you",
    "Yes",
    "No",
    "Maybe",
    "I don't know",
    "I'm not sure",
    "Do you speak English?",
    "Where is...?",
    "How much is this?",
    "I'd like... please",
    "Can I have...?",
    "Do you have...?",
    "What time is it?",
    "What's the weather like?",
    "I'm lost",
    "I'm hungry",
    "I'm thirsty",
    "Where is the restroom?",
    "I need help",
    "Call the police",
    "Call an ambulance",
    "I'm tired",
    "I'm sleepy",
    "I'm bored",
    "I'm happy",
    "I'm sad",
    "I'm angry",
    "I'm surprised",
    "I'm excited",
    "I'm scared",
    "I'm nervous",
    "I'm worried",
    "I'm confused",
    "I'm frustrated",
    "I'm disappointed",
    "I'm relieved",
    "I'm grateful",
    "I'm proud",
    "I'm ashamed",
    "I'm guilty",
    "I'm innocent",
    "I'm responsible",
    "I'm irresponsible",
    "I'm careful",
    "I'm careless",
    "I'm patient",
    "I'm impatient",
    "I'm kind",
    "I'm cruel",
    "I'm generous",
    "I'm selfish",
    "I'm honest",
    "I'm dishonest",
    "I'm trustworthy",
    "I'm untrustworthy",
    "I'm loyal",
    "I'm disloyal",
    "I'm friendly",
    "I'm unfriendly",
    "I'm polite",
    "I'm impolite",
    "I'm respectful",
    "I'm disrespectful",
    "I'm considerate",
    "I'm inconsiderate",
    "I'm thoughtful",
    "I'm thoughtless",
    "I'm helpful",
    "I'm unhelpful",
    "I'm supportive",
    "I'm unsupportive",
    "I'm encouraging",
    "I'm discouraging",
    "I'm optimistic",
    "I'm pessimistic",
    "I'm positive",
    "I'm negative",
    "I'm enthusiastic",
    "I'm unenthusiastic",
    "I'm motivated",
    "I'm unmotivated",
    "I'm focused",
    "I'm unfocused",
    "I'm determined",
    "I'm undetermined",
    "I'm confident",
    "I'm unconfident",
    "I'm courageous",
    "I'm cowardly",
    "I'm adventurous",
    "I'm unadventurous",
    "I'm spontaneous",
    "I'm unspontaneous",
    "I'm creative",
    "I'm uncreative",
    "I'm imaginative",
    "I'm unimaginative",
    "I'm innovative",
    "I'm uninnovative",
    "I'm open-minded",
    "I'm close-minded",
    "I'm flexible",
    "I'm inflexible",
    "I'm adaptable",
    "I'm unadaptable",
    "I'm resilient",
    "I'm unresilient",
    "I'm strong",
    "I'm weak",
    "I'm healthy",
    "I'm unhealthy",
    "I'm happy",
    "I'm unhappy",
    "I'm content",
    "I'm discontent",
    "I'm satisfied",
    "I'm unsatisfied",
    "I'm fulfilled",
    "I'm unfulfilled",
    "I'm successful",
    "I'm unsuccessful",
    "I'm accomplished",
    "I'm unaccomplished",
    "I'm proud",
    "I'm ashamed",
    "I'm grateful",
    "I'm ungrateful",
    "I'm hopeful",
    "I'm hopeless",
    "I'm excited",
    "I'm unexcited",
    "I'm eager",
    "I'm uneager",
    "I'm enthusiastic",
    "I'm unenthusiastic",
    "I'm motivated",
    "I'm unmotivated",
    "I'm inspired",
    "I'm uninspired" };
    //var translations = await TranslateLanguage.TranslateLanguageMethod(texts, "en", "es");

    //var listedObjects = translations.ToList().Select(x => new
    //{
    //    Original = x.Original,
    //    Translate = x.Translated,
    //    ElaspedTimeMilliSeconds = stopWatch.ElapsedMilliseconds,
    //    TotalWordsInList = texts.Count()
    //});
    //return Results.Ok(listedObjects);
    //var texts = new List<string> { "Hello, how are you?", "What is your name?", "Where are you from?" };
    var sourceLanguage = "en";
    var targetLanguage = "fr";

    
    var translatedTexts = await BulkTranslateWithAutoML.TranslateLanguageMethod(texts, sourceLanguage, targetLanguage);
    stopWatch.Stop();
    var finallst = translatedTexts.ToList().Select(x => new
    {
        x.Translated,
        x.Original,
        ElaspedTimeMilliSeconds = stopWatch.ElapsedMilliseconds,
    });
    return Results.Ok(finallst);
});

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
