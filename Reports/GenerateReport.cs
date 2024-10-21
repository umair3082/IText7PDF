using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using IText8PdfPOC.Graphs;
using IText8PdfPOC.Helpers;

namespace IText8PdfPOC.Reports
{
    public static class GenerateReport
    {
        public static MemoryStream GenerateSamplePDF() 
        {
            try
            {
                var pdfPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "PDFReports", "sample.pdf");
                //if file exists delete it
                if (File.Exists(pdfPath))
                    File.Delete(pdfPath);
                var memoryStream = new MemoryStream();
                using var Writer = new PdfWriter(memoryStream);
                using var pdfDoc = new PdfDocument(Writer);
                //Set Page Size A4
                pdfDoc.SetDefaultPageSize(PageSize.A4);
                //set Padding for the document
                var document = new Document(pdfDoc);
                //Set Margin for the document
                document.SetMargins(40, 40, 40, 40);
                // Register header event handlers
                pdfDoc.AddEventHandler(PdfDocumentEvent.START_PAGE, new HeaderEventHandler(document));

                var barcodeImg = GenerateBarCode.GetBarcodeImage("1234567890", iText.Kernel.Colors.ColorConstants.BLACK, iText.Kernel.Colors.ColorConstants.BLACK, pdfDoc);
                document.Add(barcodeImg);
                var qrCodeImg = GenerateQRCode.GetQRCodeImage("https://www.google.com", iText.Kernel.Colors.ColorConstants.BLACK, pdfDoc);
                document.Add(qrCodeImg.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT));
                document.Add(new Paragraph("Scott Plot Graph").SetBold().SetHorizontalAlignment(HorizontalAlignment.CENTER));
                var scottGraphImg = scottplotGraphs.GenerateScottPlotBars().SetHorizontalAlignment(HorizontalAlignment.CENTER);
                document.Add(scottGraphImg);
                var GenerateScottPlotImg = scottplotGraphs.GenerateScottPlotBox().SetHorizontalAlignment(HorizontalAlignment.CENTER);
                document.Add(GenerateScottPlotImg);
                document.Add(scottplotGraphs.GenerateScottPlotGroupBox());
                document.Add(scottplotGraphs.GenerateScottPlotPieSliceLabels());
                //Add Page Break
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                document.Add(new Paragraph("OxyPlot Charts").SetHorizontalAlignment(HorizontalAlignment.CENTER).SetBold());
                document.Add(OxyPlotGraphs.GenerateOxyPlotLineChart());
                document.Add(OxyPlotGraphs.GenerateOxyPlotAreaChart());
                document.Add(OxyPlotGraphs.GenerateOxyPlotPieChart());
                // Sample weather data
                List<WeatherData> weatherData = new List<WeatherData>
        {
            new WeatherData(DateTime.Now.AddDays(-5), 25, "Sunny"),
            new WeatherData(DateTime.Now.AddDays(-4), 28, "Hot"),
            new WeatherData(DateTime.Now.AddDays(-3), 2, "Cloudy"),
            //new WeatherData(DateTime.Now.AddDays(-2), 18, "Rainy"),
            new WeatherData(DateTime.Now.AddDays(-1), -2, "Cool"),
            new WeatherData(DateTime.Now, 26, "Warm"),
        };
                document.Add(OxyPlotGraphs.GenerateOxyPlotWeatherAreaChart(weatherData));
                //Add Page Break
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                // Add a table to the document
                Table table = new Table(3, false);
                table.SetMarginTop(20);
                table.SetWidth(UnitValue.CreatePercentValue(100));

                // Add headers to the table
                var dateHeaderCell = new Cell().Add(new Paragraph("Date")).SetFontColor(ColorConstants.BLACK).SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                table.AddHeaderCell(dateHeaderCell);
                var tempratureHeaderCell = new Cell().Add(new Paragraph("Temperature (C)")).SetFontColor(ColorConstants.BLACK).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(TextAlignment.CENTER);
                table.AddHeaderCell(tempratureHeaderCell);
                var summaryHeaderCell = new Cell().Add(new Paragraph("Summary")).SetFontColor(ColorConstants.BLACK).SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                table.AddHeaderCell(summaryHeaderCell);
                string[] Summaries = { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
                // Add data to the table
                Random rng = new Random();
                for (int i = 0; i < 10; i++)
                {
                    var date = DateTime.Now.AddDays(i);
                    int temperature = rng.Next(-20, 40);
                    string summary = Summaries[rng.Next(Summaries.Length)];
                    //IBlockElement dateElement = new Paragraph(date.ToShortDateString()).SetTextAlignment(TextAlignment.LEFT);
                    table.AddCell(date.ToShortDateString());
                    IBlockElement tempElement = new Paragraph(temperature.ToString()).SetTextAlignment(TextAlignment.CENTER);



                    //table.AddCell(CreateCell(temoElement,0f,0f,0f,0f));
                    table.AddCell(temperature.ToString());

                    table.AddCell(summary);
                }


              

                // Add the table to the document
                document.Add(table);


                //Row Span
                Table newtable = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
                Cell cell = new Cell().Add(new Paragraph(" 1,1 "));
                newtable.AddCell(cell);

                cell = new Cell().Add(new Paragraph(" 1,2 "));
                newtable.AddCell(cell);

                Cell cell23 = new Cell(2, 2).Add(new Paragraph("multi 1,3 and 1,4")
                    .SetTextAlignment(TextAlignment.CENTER)
                    ).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                newtable.AddCell(cell23);

                cell = new Cell().Add(new Paragraph(" 2,1 "));
                newtable.AddCell(cell);

                cell = new Cell().Add(new Paragraph(" 2,2 "));
                newtable.AddCell(cell);

                document.Add(newtable);

                //Col Span
                Table ColSpantable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2, 2, 2, 1 }));

                Cell ColSpantableCell = new Cell(2, 1).Add(new Paragraph("S/N"))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                ColSpantable.AddCell(ColSpantableCell);

                ColSpantableCell = new Cell(1, 3).Add(new Paragraph("Name")).SetTextAlignment(TextAlignment.CENTER);
                ColSpantable.AddCell(ColSpantableCell);

                ColSpantableCell = new Cell(2, 1).Add(new Paragraph("Age"))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                ColSpantable.AddCell(ColSpantableCell);

                ColSpantable.AddCell("SURNAME");
                ColSpantable.AddCell("FIRST NAME");
                ColSpantable.AddCell("MIDDLE NAME");
                ColSpantable.AddCell("1").SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                ColSpantable.AddCell("James");
                ColSpantable.AddCell("Fish");
                ColSpantable.AddCell("Stone");
                ColSpantable.AddCell("17");

                document.Add(ColSpantable);



                document.Close();
                // Set up a new PDF reader to update the document with total page count
                var reader = new PdfReader(new MemoryStream(memoryStream.ToArray()));
                var writerWithPageCount = new PdfWriter(pdfPath);
                var pdfDocWithPageCount = new PdfDocument(reader, writerWithPageCount);
                // Register footer event handler
                pdfDocWithPageCount.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandlerWithTotalPageCount(pdfDocWithPageCount));
                pdfDocWithPageCount.Close();

                return new MemoryStream(File.ReadAllBytes(pdfPath));
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        public class HeaderEventHandler : IEventHandler
        {
            private Document document;

            public HeaderEventHandler(Document document)
            {
                this.document = document;
            }

            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfPage page = docEvent.GetPage();
                PdfDocument pdfDoc = docEvent.GetDocument();
                //get event type
                var eventype = docEvent.GetEventType();

                iText.Kernel.Geom.Rectangle pageSize = page.GetPageSize();

                // Create a PdfCanvas to draw on the PDF page
                PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
                // Create a Canvas object using the correct constructor
                Canvas canvas = new Canvas(pdfCanvas, pageSize);
                // Add a title to the document
                if (eventype == "StartPdfPage")
                {
                    canvas.Add(new Paragraph("Weather Condition Report")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                    .SetFontSize(24)
                    .SetMarginBottom(10)
                );
                    //LineSeparator ls = new LineSeparator(new SolidLine());
                    //canvas.Add(ls)
                    //    .SetStrokeWidth(5.6f)
                    //    .SetStrokeColor(DeviceRgb.RED);
                }
                canvas.Close();
            }
        }

        //Footer Event Handler
        public class FooterEventHandler : IEventHandler
        {
            private readonly Document document;

            public FooterEventHandler(Document document)
            {
                this.document = document;
            }

            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfPage page = docEvent.GetPage();
                PdfDocument pdfDoc = docEvent.GetDocument();
                Rectangle pageSize = page.GetPageSize();

                PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
                Canvas canvas = new Canvas(pdfCanvas, pageSize);

                // Add the footer content (page number)
                canvas.ShowTextAligned(
                    "Page " + pdfDoc.GetPageNumber(page) + " of " + pdfDoc.GetNumberOfPages(),
                    pageSize.GetWidth() / 2,
                    pageSize.GetBottom() + 20,
                    TextAlignment.CENTER);

                canvas.Close();
            }
        }
        public class FooterEventHandlerWithTotalPageCount : IEventHandler
        {
            private readonly PdfDocument pdfDoc;

            public FooterEventHandlerWithTotalPageCount(PdfDocument pdfDoc)
            {
                this.pdfDoc = pdfDoc;
            }

            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfPage page = docEvent.GetPage();
                Rectangle pageSize = page.GetPageSize();

                PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
                Canvas canvas = new Canvas(pdfCanvas, pageSize);

                // Add the footer content (page number)
                canvas.ShowTextAligned(
                    "Page " + pdfDoc.GetPageNumber(page) + " of " + pdfDoc.GetNumberOfPages(),
                    pageSize.GetWidth() / 2,
                    pageSize.GetBottom() + 20,
                    TextAlignment.CENTER);

                canvas.Close();
            }
        }

    }
}
