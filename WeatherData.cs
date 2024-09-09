namespace IText8PdfPOC
{
    public class WeatherData
    {
        public DateTime Date { get; set; }
        public int Temperature { get; set; }
        public string Summary { get; set; }

        public WeatherData(DateTime date, int temperature, string summary)
        {
            Date = date;
            Temperature = temperature;
            Summary = summary;
        }
    }
}
