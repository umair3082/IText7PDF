
using Microsoft.Reporting.NETCore;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using ProductSales;

namespace IText7PdfPOC.Reports.RDLC_Reports
{
    public class TestRdlcReport : Controller
    {
        public TestRdlcReport()
        {

        }
        public IActionResult Preview()
        {
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "HelloWorld.rdlc");
            var report = new LocalReport { ReportPath = reportPath };

            // Optionally, set data sources if needed
            // report.DataSources.Add(new ReportDataSource("DataSourceName", data));

            // Render the report to a byte array
            var reportBytes = report.Render("PDF");

            // Return the report as a file
            return File(reportBytes, "application/pdf", "HelloWorld.pdf");
        }
    }
}
