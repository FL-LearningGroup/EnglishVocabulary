using EnglishVocabulary.Core.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace EnglishVocabulary.Core
{
    public class WordLoader: IWordLoader, IDisposable
    {
        private HttpClient _httpClient;
        public WordLoader()
        {
            _httpClient = new HttpClient();
        }
        public async Task<List<string>> LoadWordFromFileAsync(string path)
        {
            List<string> wordList = new List<string>();
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path + "not been found.");
            }
            wordList = (await File.ReadAllLinesAsync(path)).ToList();
            return wordList;
        }

        public async Task GetWordMp3FileAsync(Word word)
        {
            HttpResponseMessage responseMessage =  await _httpClient.GetAsync(word.RemoteAudio);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Get {word.EnglishLanguage} mp3 media failed.");
            }
            using (Stream stream = await responseMessage.Content.ReadAsStreamAsync())
            {
                FileInfo fileInfo = new FileInfo(word.LocalAudio);

                if (!fileInfo.Directory.Exists) fileInfo.Directory.Create();

                if (fileInfo.Exists) fileInfo.Delete();

                word.LocalAudio = fileInfo.FullName;
                using (FileStream fileStream = fileInfo.OpenWrite())
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
        }

        public async Task StorageTranslatedWordAsync(List<Word> wordList, string path)
        {
            var encoderSettings = new TextEncoderSettings();
            encoderSettings.AllowCharacters('\u0027'); //Invalid
            encoderSettings.AllowRange(UnicodeRanges.All);
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(encoderSettings),
                WriteIndented = true
            };
            string txt = JsonSerializer.Serialize(wordList, options);
            txt = txt.Replace("\\u0027", "'");
            await File.WriteAllTextAsync(path,txt);
        }

        public async Task StorageTranslatedWordToExcelAsync(List<Word> wordList, string excelPath, string sheetName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; //Set excel license

            FileInfo excelFile = new FileInfo(excelPath);

            using (ExcelPackage excel = new ExcelPackage(excelFile))
            {
                var sheet = excel.Workbook.Worksheets[sheetName];
                if (sheet == null) sheet = excel.Workbook.Worksheets.Add(sheetName);
                int headerIndex = 1;
                int recordIndex = 2;
                foreach (var property in typeof(Word).GetProperties())
                {
                    sheet.Cells[1, headerIndex].Value = property.Name;
                    headerIndex++;
                }
                foreach (var record in wordList)
                {
                    sheet.Cells[recordIndex, 1].Value = record.EnglishLanguage;
                    sheet.Cells[recordIndex, 2].Value = record.ChineseLanguage;
                    sheet.Cells[recordIndex, 3].Value = record.Phonetic;
                    sheet.Cells[recordIndex, 4].Value = record.Tense;
                    sheet.Cells[recordIndex, 5].Value = record.RemoteAudio;
                    sheet.Cells[recordIndex, 6].Value = record.LocalAudio;
                    sheet.Cells[recordIndex, 7].Value = record.TranslatedMsg;
                    recordIndex++;
                }

                await excel.SaveAsync();
            }
        }
        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
