using GoogleTranslateFreeApi;
using System.Text.RegularExpressions;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace IText7PdfPOC.LangTranslator
{
    public static class TranslateLanguage
    {

        public static async Task<List<(string Original, string Translated)>> TranslateLanguageMethod(List<string> texts, string sourceLanguage, string targetLanguage)
        {
            using (HttpClient client = new HttpClient())
            {
                var tasks = new List<Task<(string Original, string Translated)>>();

                foreach (var text in texts)
                {
                    tasks.Add(TranslateSingleTextAsync(client, text, sourceLanguage, targetLanguage));
                }

                // Wait for all translations to complete
                var results = await Task.WhenAll(tasks);
                return results.ToList();
            }
        }

        private static async Task<(string Original, string Translated)> TranslateSingleTextAsync(HttpClient client, string text, string sourceLanguage, string targetLanguage)
        {
            string encodedText = Uri.EscapeDataString(text);
            string apiUrl = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLanguage}&tl={targetLanguage}&dt=t&q={encodedText}";

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                string decodedResponse = Regex.Replace(responseContent.Split('"')[1], @"\\u(?<Value>[a-zA-Z0-9]{4})", m => ((char)int.Parse(m.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber)).ToString());
                return (text, decodedResponse); // Return original text and translated text
            }
            else
            {
                return (text, "Error"); // Return original text and an error message
            }
        }
        ////public static async Task<List<string>> TranslateLanguageMethod(List<string> texts, string sourceLanguage, string targetLanguage)
        ////{
        ////    using (HttpClient client = new HttpClient())
        ////    {
        ////        var tasks = new List<Task<string>>();

        ////        foreach (var text in texts)
        ////        {
        ////            tasks.Add(TranslateSingleTextAsync(client, text, sourceLanguage, targetLanguage));
        ////        }

        ////        // Wait for all translations to complete
        ////        //return await Task.WhenAll(tasks);
        ////        var results = await Task.WhenAll(tasks);
        ////        return new List<string>(results);
        ////    }
        ////}

        ////private static async Task<string> TranslateSingleTextAsync(HttpClient client, string text, string sourceLanguage, string targetLanguage)
        ////{
        ////    string encodedText = Uri.EscapeDataString(text);
        ////    string apiUrl = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLanguage}&tl={targetLanguage}&dt=t&q={encodedText}";

        ////    HttpResponseMessage response = await client.GetAsync(apiUrl);

        ////    if (response.IsSuccessStatusCode)
        ////    {
        ////        string responseContent = await response.Content.ReadAsStringAsync();
        ////        string decodedResponse = Regex.Replace(responseContent.Split('"')[1], @"\\u(?<Value>[a-zA-Z0-9]{4})", m => ((char)int.Parse(m.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber)).ToString());
        ////        return decodedResponse;
        ////    }
        ////    else
        ////    {
        ////        return "Error";
        ////    }
        ////}
        //private static readonly GoogleTranslator Translator = new GoogleTranslator();

        ////public static async Task<string> TranslateLanguageMethod(List<string> text,string sourceLanguage,string targetLanguage)
        ////{
        ////    using (HttpClient client = new HttpClient())
        ////    {

        ////        //string apiUrl = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLanguage}&tl={targetLanguage}&dt=t&q={Uri.EscapeUriString(item.ToString())}";
        ////        string encodedText = text;
        ////        string apiUrl = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLanguage}&tl={targetLanguage}&dt=t&q={Uri.EscapeDataString(encodedText)}";
        ////        HttpResponseMessage response = await client.GetAsync(apiUrl);

        ////        if (response.IsSuccessStatusCode)
        ////        {
        ////            string responseContent = await response.Content.ReadAsStringAsync();
        ////            // Assuming responseText contains the API response
        ////            string decodedResponse = Regex.Replace(responseContent.Split('"')[1], @"\\u(?<Value>[a-zA-Z0-9]{4})", m => ((char)int.Parse(m.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber)).ToString());
        ////            return decodedResponse;
        ////            //TranslationListBox.Items.Add(decodedResponse);
        ////            //TranslationListBox.TopIndex = TranslationListBox.Items.Count - 1;
        ////            //Console.WriteLine($"Original Text: {inputText}");
        ////            //Console.WriteLine($"Translated Text: {translatedText}");
        ////        }
        ////        else
        ////        {
        ////           return "Error";
        ////        }
        ////    }
        ////}


        //public static async Task<List<string>> TranslateLanguageMethod(List<string> texts, string sourceLanguage, string targetLanguage)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        var tasks = new List<Task<string>>();

        //        foreach (var text in texts)
        //        {
        //            tasks.Add(TranslateSingleTextAsync(client, text, sourceLanguage, targetLanguage));
        //        }

        //        // Wait for all translations to complete
        //        var results = await Task.WhenAll(tasks);
        //        return new List<string>(results);
        //    }
        //}

        //private static async Task<string> TranslateSingleTextAsync(HttpClient client, string text, string sourceLanguage, string targetLanguage)
        //{
        //    string encodedText = Uri.EscapeDataString(text);
        //    string apiUrl = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLanguage}&tl={targetLanguage}&dt=t&q={encodedText}";

        //    HttpResponseMessage response = await client.GetAsync(apiUrl);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string responseContent = await response.Content.ReadAsStringAsync();
        //        string decodedResponse = Regex.Replace(responseContent.Split('"')[1], @"\\u(?<Value>[a-zA-Z0-9]{4})", m => ((char)int.Parse(m.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber)).ToString());
        //        return decodedResponse;
        //    }
        //    else
        //    {
        //        return "Error";
        //    }
        //}

    }
}
