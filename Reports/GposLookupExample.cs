using System.IO;
using iText.IO.Font;
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
using IText8PdfPOC;
using static IText8PdfPOC.Reports.GenerateReport;
using iText.IO.Font.Constants;
using iText.Layout.Font;
using System.Xml;
using iText.Licensing.Base;


//using iText.Licensing.Base;

namespace IText7PdfPOC.Reports
{
    public static class GposLookupExample
    {
        private static string TEXT =
            "ဗမာမှာ လူများစုဗမာမျိုး နွယ်စု၏ ခေါ် ရာတွင် တရားဝင်ခေါ် သော အသုံး ဖြ စ်သည်။ အင်္ဂလိပ်လက်အော က်";
        private static string UrduText = "چونکہ انسانی حقوق سے لاپروائی اور ان کی بے حرمتی اکثر ایسے وحشیانہ";

        private static string FONT = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Fonts", "NotoSansMyanmar-Regular.ttf");
        private static string UrduFont = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Fonts", @"ALQALAM_ALVI_NASTALEEQ_SHIPPED.ttf");
        private static string DEST = "result.pdf";
        public static MemoryStream ManipulatePdf()
        {
            ////var pdfPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "PDFReports", "sample.pdf");
            //////if file exists delete it
            ////if (File.Exists(pdfPath))
            ////    File.Delete(pdfPath);
            ////var memoryStream = new MemoryStream();
            ////string[] sources = new string[] { "english.xml", "arabic.xml", "hindi.xml", "tamil.xml" };
            ////PdfWriter writer = new PdfWriter(DEST);
            ////PdfDocument pdfDocument = new PdfDocument(writer);
            ////Document document = new Document(pdfDocument);
            ////FontSet set = new FontSet();
            ////set.AddFont("NotoNaskhArabic-Regular.ttf");
            ////set.AddFont("NotoSansTamil-Regular.ttf");
            ////set.AddFont("FreeSans.ttf");
            ////document.SetFontProvider(new FontProvider(set));
            ////document.SetProperty(Property.FONT, new String[] { "MyFontFamilyName" });
            ////foreach (string source in sources)
            ////{
            ////    XmlDocument doc = new XmlDocument();
            ////    var xlmpath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Lang_xml", source);
            ////    var stream = new FileStream(xlmpath, FileMode.Open);
            ////    doc.Load(stream);
            ////    XmlNode element = doc.GetElementsByTagName("text").Item(0);
            ////    Paragraph paragraph = new Paragraph();
            ////    XmlNode textDirectionElement = element.Attributes.GetNamedItem("direction");
            ////    Boolean rtl = textDirectionElement != null && textDirectionElement.InnerText.Equals("rtl");
            ////    if (rtl)
            ////    {
            ////        paragraph.SetTextAlignment(TextAlignment.RIGHT);
            ////    }
            ////    paragraph.Add(element.InnerText);
            ////    document.Add(paragraph);
            ////}
            ////document.Close();
            ////var reader = new PdfReader(new MemoryStream(memoryStream.ToArray()));
            ////var writerWithPageCount = new PdfWriter(pdfPath);
            ////var pdfDocWithPageCount = new PdfDocument(reader, writerWithPageCount);
            ////// Register footer event handler
            ////pdfDocWithPageCount.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandlerWithTotalPageCount(pdfDocWithPageCount));
            ////pdfDocWithPageCount.Close();

            ////return new MemoryStream(File.ReadAllBytes(pdfPath));
            try
            {
                LicenseKey.LoadLicenseFile(new FileInfo(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "96e701620b36d6f1b2f73271dfb8478ced614076f57998214938b1abb03207cb.json")));
                var pdfPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "PDFReports", "sample.pdf");
                //if file exists delete it
                if (File.Exists(pdfPath))
                    File.Delete(pdfPath);
                var memoryStream = new MemoryStream();
                using var Writer = new PdfWriter(memoryStream);
                using var pdfDoc = new PdfDocument(Writer);
                //Set Page Size A4
                pdfDoc.SetDefaultPageSize(PageSize.A4);
                //pdfDoc.AddExtension(new PdfCalligraphExtension());
                //set Padding for the document
                var document = new Document(pdfDoc);
                //Set Margin for the document
                document.SetMargins(40, 40, 40, 40);
                // Register header event handlers
                pdfDoc.AddEventHandler(PdfDocumentEvent.START_PAGE, new HeaderEventHandler(document));


                PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H,
                        PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
                Paragraph paragraph = new Paragraph(TEXT).SetBaseDirection(BaseDirection.RIGHT_TO_LEFT)
                    .SetTextAlignment(TextAlignment.CENTER);
                font.SetSubset(false);
                paragraph.SetFont(font);
                document.Add(paragraph);

                document.Add(new Paragraph(UrduText.ToString()));

                ////Urdu Font
                //PdfFont urdufont = PdfFontFactory.CreateFont(UrduFont, PdfEncodings.IDENTITY_H, 
                //        PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
                //Paragraph Urduparagraph = new Paragraph(UrduText).SetBaseDirection(BaseDirection.RIGHT_TO_LEFT)
                //  .SetTextAlignment(TextAlignment.LEFT);
                //urdufont.SetSubset(false);
                //Urduparagraph.SetFont(urdufont);
                //document.Add(Urduparagraph);


                PdfFont ufont = PdfFontFactory.CreateFont(UrduFont, PdfEncodings.IDENTITY_H,
                       PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);

                Paragraph uparagraph = new Paragraph(UrduText).SetBaseDirection(BaseDirection.LEFT_TO_RIGHT)
                    .SetFontColor(ColorConstants.BLACK)
                    .SetBackgroundColor(ColorConstants.PINK)
                    .SetTextAlignment(TextAlignment.CENTER);
                ufont.SetSubset(false);
                uparagraph.SetFont(ufont);
                document.Add(uparagraph);
                //// Urdu Font with Calligraph support
                //PdfFont ufont = PdfFontFactory.CreateFont(UrduFont, PdfEncodings.IDENTITY_H,
                //    PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
                //Paragraph uparagraph = new Paragraph(UrduText)
                //    .SetBaseDirection(BaseDirection.RIGHT_TO_LEFT)
                //    .SetFontColor(ColorConstants.BLACK)
                //    .SetBackgroundColor(ColorConstants.PINK)
                //    .SetTextAlignment(TextAlignment.RIGHT);
                //uparagraph.SetFont(ufont);
                //document.Add(uparagraph);


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
            //var pdfPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "PDFReports", "sample.pdf");
            ////if file exists delete it
            //if (File.Exists(pdfPath))
            //    File.Delete(pdfPath);
            //var memoryStream = new MemoryStream();
            //using var Writer = new PdfWriter(memoryStream);
            //using var pdfDoc = new PdfDocument(Writer);
            ////Set Page Size A4
            //pdfDoc.SetDefaultPageSize(PageSize.A4);
            ////set Padding for the document
            //var document = new Document(pdfDoc);
            ////Set Margin for the document
            //document.SetMargins(40, 40, 40, 40);

            //PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H,
            //        PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
            //    Paragraph paragraph = new Paragraph(TEXT).SetBaseDirection(BaseDirection.LEFT_TO_RIGHT)
            //        .SetTextAlignment(TextAlignment.LEFT);
            //    font.SetSubset(false);
            //    paragraph.SetFont(font);
            //    document.Add(paragraph);
            //document.Close();

            //var reader = new PdfReader(new MemoryStream(memoryStream.ToArray()));
            //var writerWithPageCount = new PdfWriter(pdfPath);
            //var pdfDocWithPageCount = new PdfDocument(reader, writerWithPageCount);

            //return new MemoryStream(File.ReadAllBytes(pdfPath));
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
