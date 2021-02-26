using EnglishVocabulary.Core;
using EnglishVocabulary.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;


namespace EnglishVocabulary.Interactive
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IWordLoader wordLoader = new WordLoader();
            IWordTranslator wordTranslator = new WordTranslator();
            List<Word> wordList = new List<Word>();
            string storageFolder = @"D:\Lucas\EnglishVocabulary\level4";
            List<string> engWord =  await wordLoader.LoadWordFromFileAsync(@"D:\Lucas\git\EnglishVocabulary\resources\Level4Vocabulary\level4-01.txt");
            foreach (string word in engWord)
            {
                wordList.Add(await wordTranslator.TranslateAsync(word));
            }
            //wordList.ForEach(item => item.LocalAudio = storageFolder + @"\audio\" + item.EnglishLanguage + ".mp3" );
            try
            {
                //wordList.ForEach(async item => await wordLoader.GetWordMp3FileAsync(item));
            }
            catch(HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
            await wordLoader.StorageTranslatedWordAsync(wordList, @"D:\Lucas\EnglishVocabulary\level4\level4-01.json");
            await wordLoader.StorageTranslatedWordToExcelAsync(wordList, @"D:\Lucas\EnglishVocabulary\level4\level4.xlsx", "level4-02");
            Console.ReadKey();
        }
    }
}
