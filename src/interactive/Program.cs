using EnglishVocabulary.Core;
using EnglishVocabulary.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;


namespace EnglishVocabulary.Interactive
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IWordLoader wordLoader = new WordLoader();
            IWordTranslator wordTranslator = new WordTranslator();
            List<string> wordList = wordLoader.LoadWordFromFile(WordStorageType.Excel, @"D:\Lucas\git\EnglishVocabulary\resources\EnglishVocabulary.xlsx");
            Word word = await wordTranslator.TranslateAsync(wordList[0]);
            await wordLoader.LoadWordMp3FileAsync(word);
            //Console.ReadKey();
        }
    }
}
