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

namespace EnglishVocabulary.Core
{
    public class WordLoader: IWordLoader, IDisposable
    {
        private HttpClient _httpClient;
        public WordLoader()
        {
            _httpClient = new HttpClient();
        }
        public List<string> LoadWordFromFile(WordStorageType type, string path)
        {
            List<string> wordList = new List<string>();

            if (type == WordStorageType.Excel)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                FileInfo fileInfo = new FileInfo(path);
                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    // Skip header by set index to 2.
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        wordList.Add(worksheet.Cells[row, 1].Value.ToString().Trim());
                    }

                }
            }
            return wordList;
        }

        public async Task LoadWordMp3FileAsync(Word word)
        {
            HttpResponseMessage responseMessage =  await _httpClient.GetAsync(word.RemoteAudio);
            using (Stream stream = await responseMessage.Content.ReadAsStreamAsync())
            {
                FileInfo fileInfo = new FileInfo(@"D:\Lucas\git\EnglishVocabulary\resources\Audio\" + word.EnglishLanguage + ".mp3");
                using (FileStream fileStream = fileInfo.OpenWrite())
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
