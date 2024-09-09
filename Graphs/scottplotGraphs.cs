using IText8PdfPOC.Helpers;
using ScottPlot;

namespace IText8PdfPOC.Graphs
{
    public static class scottplotGraphs
    {
        public static iText.Layout.Element.Image GenerateScottPlotBars()
        {
            Plot myPlot = new();

            double[] xs1 = { 1, 2, 3, 4 };
            double[] ys1 = { 5, 10, 7, 13 };
            var bars1 = myPlot.Add.Bars(xs1, ys1);
            bars1.LegendText = "Alpha";

            double[] xs2 = { 6, 7, 8, 9 };
            double[] ys2 = { 7, 12, 9, 15 };
            var bars2 = myPlot.Add.Bars(xs2, ys2);
            bars2.LegendText = "Beta";

            myPlot.ShowLegend(Alignment.UpperLeft);
            myPlot.Axes.Margins(bottom: 0);

            myPlot.SavePng("demo.png", 400, 300);
            byte[] imageBytes = myPlot.GetImageBytes(400, 300, ScottPlot.ImageFormat.Png);
            using (MemoryStream imageStream = new MemoryStream(imageBytes))
            {
                return ConvertToITextImageClass.ConvertBytesToITextImage(imageBytes);
            }
        }
        public static iText.Layout.Element.Image GenerateScottPlotBox()
        {
            Plot myplot = new Plot();
            ScottPlot.Box box = new()
            {
                Position = 5,
                BoxMin = 81,
                BoxMax = 99,
                WhiskerMin = 75,
                WhiskerMax = 105,
                BoxMiddle = 90,
            };
            myplot.Add.Box(box);
            myplot.Axes.SetLimits(0, 10, 70, 110);
            myplot.SavePng("boxplot.png", 400, 300);
            byte[] imageBytes = myplot.GetImageBytes(400, 300, ScottPlot.ImageFormat.Png);
            using (MemoryStream imageStream = new MemoryStream(imageBytes))
            {
                return ConvertToITextImageClass.ConvertBytesToITextImage(imageBytes);
            }
        }
        public static iText.Layout.Element.Image GenerateScottPlotGroupBox()
        {
            ScottPlot.Plot myPlot = new();

            List<ScottPlot.Box> boxes1 = new() {
                Generate.RandomBox(1),
                Generate.RandomBox(2),
                Generate.RandomBox(3),
            };

            List<ScottPlot.Box> boxes2 = new() {
                Generate.RandomBox(5),
                Generate.RandomBox(6),
                Generate.RandomBox(7),
            };

            var bp1 = myPlot.Add.Boxes(boxes1);
            bp1.LegendText = "Group 1";

            var bp2 = myPlot.Add.Boxes(boxes2);
            bp2.LegendText = "Group 2";

            myPlot.ShowLegend(Alignment.UpperRight);

            myPlot.SavePng("demo.png", 400, 300);
            byte[] imageBytes = myPlot.GetImageBytes(400, 300, ScottPlot.ImageFormat.Png);
            using (MemoryStream imageStream = new MemoryStream(imageBytes))
            {
                return ConvertToITextImageClass.ConvertBytesToITextImage(imageBytes);
            }
        }
        public static iText.Layout.Element.Image GenerateScottPlotPieSliceLabels()
        {
            ScottPlot.Plot myPlot = new();

            PieSlice slice1 = new() { Value = 5, FillColor = Colors.Red, Label = "Red" };
            PieSlice slice2 = new() { Value = 2, FillColor = Colors.Orange, Label = "Orange" };
            PieSlice slice3 = new() { Value = 8, FillColor = Colors.Gold, Label = "Yellow" };
            PieSlice slice4 = new() { Value = 4, FillColor = Colors.Green, Label = "Green" };
            PieSlice slice5 = new() { Value = 8, FillColor = Colors.Blue, Label = "Blue" };
            List<PieSlice> slices = new() { slice1, slice2, slice3, slice4, slice5 };

            // setup the pie to display slice labels
            var pie = myPlot.Add.Pie(slices);
            pie.ExplodeFraction = .1;
            pie.ShowSliceLabels = true;
            pie.SliceLabelDistance = 1.3;

            // styling can be customized for individual slices
            slice5.LabelStyle.FontSize = 22;
            slice5.LabelStyle.ForeColor = Colors.Magenta;
            slice5.LabelStyle.Bold = true;

            myPlot.SavePng("demopie.png", 600, 400);
            byte[] imageBytes = myPlot.GetImageBytes(400, 300, ScottPlot.ImageFormat.Png);
            using (MemoryStream imageStream = new MemoryStream(imageBytes))
            {
                return ConvertToITextImageClass.ConvertBytesToITextImage(imageBytes);
            }
        }
    }
}
