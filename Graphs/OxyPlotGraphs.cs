using iText.IO.Image;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.SkiaSharp;
using System.Drawing;

namespace IText8PdfPOC.Graphs
{
    public static class OxyPlotGraphs
    {
        public static iText.Layout.Element.Image GenerateOxyPlotLineChart()
        {
            // Generate chart as an image
            var plotModel = new PlotModel { Title = "Line Chart Example" };

            plotModel.Series.Add(new LineSeries
            {
                Title = "Series 1",
                Points = { new DataPoint(0, 0), new DataPoint(10, 18), new DataPoint(20, 12) },
            });

            // Use OxyPlot to render the chart to a stream
            using (var chartStream = new MemoryStream())
            {
                var pngExporter = new PngExporter { Width = 600, Height = 400 };
                pngExporter.Export(plotModel, chartStream);
                chartStream.Position = 0; // Reset the stream position

                // Convert chart stream to an iText7 image
                var chartImage = new iText.Layout.Element.Image(ImageDataFactory.Create(chartStream.ToArray()));

                return chartImage;
            }
        }
        public static iText.Layout.Element.Image GenerateOxyPlotAreaChart()
        {
            int width = 600;
            int height = 400;
            Random rng = new Random();

            using (Bitmap bitmap = new Bitmap(width, height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // Clear the background
                    graphics.Clear(System.Drawing.Color.White);

                    // Set up pens, brushes, and fonts
                    Pen pen = new Pen(System.Drawing.Color.Black, 2);
                    Brush brush = Brushes.LightBlue; // Brush for the area fill
                    Font font = new Font("Arial", 10);
                    Brush textBrush = Brushes.Black;

                    // Draw axes
                    graphics.DrawLine(pen, 50, 350, 550, 350); // X-Axis
                    graphics.DrawLine(pen, 50, 350, 50, 50);   // Y-Axis

                    // Data points
                    PointF[] points = new PointF[]
                    {
                new PointF(50, 300),
                new PointF(150, 250),
                new PointF(250, 200),
                new PointF(350, 150),
                new PointF(450, 400),
                new PointF(550, 50)
                    };

                    // Create an array for the area fill points, including the baseline
                    PointF[] areaPoints = new PointF[points.Length + 2];
                    areaPoints[0] = new PointF(points[0].X, 350); // Start at the X-axis
                    areaPoints[areaPoints.Length - 1] = new PointF(points[points.Length - 1].X, 350); // End at the X-axis

                    // Copy the original points into the areaPoints array
                    Array.Copy(points, 0, areaPoints, 1, points.Length);

                    // Fill the area
                    graphics.FillPolygon(brush, areaPoints);

                    // Draw the line over the filled area
                    graphics.DrawLines(pen, points);

                    // Draw labels for each point
                    for (int i = 0; i < points.Length; i++)
                    {
                        string label = $"P{i + 1}";
                        graphics.DrawString(label, font, textBrush, points[i].X - 10, points[i].Y - 20);
                    }
                }

                // Convert the Bitmap to a byte array
                using (MemoryStream chartStream = new MemoryStream())
                {
                    bitmap.Save(chartStream, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = chartStream.ToArray();

                    // Create iText7 ImageData and Image from the byte array
                    ImageData imageData = ImageDataFactory.Create(imageBytes);
                    return new iText.Layout.Element.Image(imageData).SetAutoScale(true);
                }
            }
        }
        public static iText.Layout.Element.Image GenerateOxyPlotWeatherAreaChart(List<WeatherData> weatherData)
        {
            int width = 600;
            int height = 400;

            using (Bitmap bitmap = new Bitmap(width, height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // Clear the background
                    graphics.Clear(System.Drawing.Color.White);

                    // Set up pens, brushes, and fonts
                    Pen pen = new Pen(System.Drawing.Color.Black, 2);
                    Brush areaBrush = Brushes.LightBlue; // Brush for the area fill
                    Font font = new Font("Arial", 10);
                    Brush textBrush = Brushes.Black;

                    // Draw axes
                    graphics.DrawLine(pen, 50, 350, 550, 350); // X-Axis
                    graphics.DrawLine(pen, 50, 350, 50, 50);   // Y-Axis

                    // Prepare points for the area chart
                    PointF[] points = new PointF[weatherData.Count];
                    PointF[] areaPoints = new PointF[weatherData.Count + 2];
                    areaPoints[0] = new PointF(50, 350); // Start at the X-axis
                    areaPoints[areaPoints.Length - 1] = new PointF(550, 350); // End at the X-axis

                    float xStep = 500f / (weatherData.Count - 1);
                    for (int i = 0; i < weatherData.Count; i++)
                    {
                        float x = 50 + i * xStep;
                        float y = 350 - (weatherData[i].Temperature * 3); // Scale temperature

                        points[i] = new PointF(x, y);
                        areaPoints[i + 1] = points[i];

                        // Draw labels for each point
                        //{weatherData[i].Date.ToShortDateString()}: 
                        string label = $"{weatherData[i].Temperature}°C\n{weatherData[i].Summary}";
                        graphics.DrawString(label, font, textBrush, x - 30, y - 40);
                    }

                    // Fill the area under the line
                    graphics.FillPolygon(areaBrush, areaPoints);

                    // Draw the line over the filled area
                    graphics.DrawLines(pen, points);
                }

                // Convert the Bitmap to a byte array
                using (MemoryStream chartStream = new MemoryStream())
                {
                    bitmap.Save(chartStream, System.Drawing.Imaging.ImageFormat.Png);
                    chartStream.Position = 0;

                    // Create iText7 ImageData and Image from the byte array
                    ImageData imageData = ImageDataFactory.Create(chartStream.ToArray());
                    return new iText.Layout.Element.Image(imageData).SetAutoScale(true);
                }
            }
        }
        public static iText.Layout.Element.Image GenerateOxyPlotPieChart()
        {
            var plotModel = new PlotModel { Title = "Sample Pie Chart" };

            var pieSeries = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0,
                TextColor = OxyColors.Black,
                InsideLabelColor = OxyColors.White,
                OutsideLabelFormat = "{0}: {1}"
            };
            Random rnd = new Random();
            var listOfColors = new List<OxyColor> { };
            pieSeries.Slices.Add(new PieSlice("Category A", 40) { IsExploded = true, Fill = OxyColors.Red });
            pieSeries.Slices.Add(new PieSlice("Category B", 30) { Fill = OxyColors.Blue });
            pieSeries.Slices.Add(new PieSlice("Category C", 20) { Fill = OxyColors.Green });
            pieSeries.Slices.Add(new PieSlice("Category D", 10) { Fill = OxyColors.Yellow });

            plotModel.Series.Add(pieSeries);
            // Render the plot to a bitmap image
            var pngExporter = new PngExporter { Width = 600, Height = 400 };
            using (var stream = new MemoryStream())
            {
                pngExporter.Export(plotModel, stream);

                // Convert the bitmap stream to an iTextSharp Image
                var itextImage = new iText.Layout.Element.Image(ImageDataFactory.Create(stream.ToArray()));
                return itextImage;
            }

        }
    }
}
