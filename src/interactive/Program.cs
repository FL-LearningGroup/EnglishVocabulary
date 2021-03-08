using EnglishVocabulary.Core;
using EnglishVocabulary.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace EnglishVocabulary.Interactive
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //string subFolder = "level4-05";
            IWordLoader wordLoader = new WordLoader();
            IWordTranslator wordTranslator = new WordTranslator();
            List<Word> wordList = new List<Word>();
            string storageFolder = @"D:\Lucas\EnglishVocabulary\level4";
            IEnumerable<string> engWordTemplate = wordLoader.LoadWordFromExcelAsync(@"C:\Users\v-diya\Desktop\EnglishLevel4.xlsx");//(await wordLoader.LoadWordFromFileAsync(@$"D:\Lucas\git\EnglishVocabulary\resources\Level4Vocabulary\{subFolder}.txt")).ToList();

            var engWord = from word in engWordTemplate
                          orderby word ascending
                          select word;


            //int skipDebug = 0;
            foreach (string word in engWord)
            {
                //if (skipDebug > 57) break;
                wordList.Add(await wordTranslator.TranslateAsync(word));
                //skipDebug++;
            }

            wordList.ForEach(item => item.LocalAudio = storageFolder + $"\\audio\\" + item.EnglishLanguage + ".mp3");

            await wordLoader.StorageTranslatedWordAsync(wordList, 100, storageFolder, "level4");
            await wordLoader.StorageTranslatedWordToExcelAsync(wordList,100, storageFolder+@"\level4.xlsx", "level4");

            try
            {
                foreach (var item in wordList)
                {
                    await wordLoader.GetWordMp3FileAsync(item);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
