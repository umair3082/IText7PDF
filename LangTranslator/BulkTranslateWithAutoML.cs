using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IText7PdfPOC.LangTranslator
{
    public static class BulkTranslateWithAutoML
    {
        //Static method to translate language
        public static async Task<List<(string Original, string Translated)>> TranslateLanguageMethod(List<string> texts, string sourceLanguage, string targetLanguage)
        {
            var autoMLModelId = "general/nmt";
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&dt=t&sl={sourceLanguage}&tl={targetLanguage}&q={string.Join("%0A", texts)}&model={autoMLModelId}";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var responsessssssssssssss = JsonConvert.DeserializeObject<List<object>>(responseBody);
                var translations = ((JArray)responsessssssssssssss.ToArray()[0]);
                var translatedTexts = new List<(string Original, string Translated)>();

                for (int i = 0; i < texts.Count; i++)
                {
                    // Accessing the translated text and original text
                    var translatedJsonText = translations[i].ToString();
                    var translationEntry = JsonConvert.DeserializeObject<List<object>>(translatedJsonText);
                    var _TranslatedText = translationEntry[0]?.ToString() ?? string.Empty;
                    var originalText = texts[i];
                    translatedTexts.Add((originalText, _TranslatedText));
                }
                return translatedTexts;
            }
        }
    }
    public class RootObject
    {
        public List<string> Strings { get; set; }
        public List<object> Nulls { get; set; }
        public int? Number { get; set; }
        public List<List<List<object>>> NestedArray { get; set; }
    }

    public class TranslationEntry
    {
        public string? TranslatedText { get; set; }
        public string? OriginalText { get; set; }
        public object? Confidence { get; set; }
        public object? Tool { get; set; }
        public int? Score { get; set; }
        public object? Alignment { get; set; }
        public object? AlignmentQuality { get; set; }
        public object? AlignmentType { get; set; }
        public List<List<object>> Synonyms { get; set; }
    }

    public class TranslationResponse
    {
        public List<List<object>> Translations { get; set; }
        public string? SourceLanguage { get; set; }
    }

    public class Translation
    {
        public string? TranslatedText { get; set; }
        public string? OriginalText { get; set; }
        public object? Confidence { get; set; }
        public object? Tool { get; set; }
        public int? Score { get; set; }
        public object? Alignment { get; set; }
        public object? AlignmentQuality { get; set; }
        public object? AlignmentType { get; set; }
        public List<List<object>> Synonyms { get; set; }
    }
}