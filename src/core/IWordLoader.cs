using EnglishVocabulary.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVocabulary.Core
{
    public interface IWordLoader
    {
        Task<List<string>> LoadWordFromFileAsync(string path);
        Task GetWordMp3FileAsync(Word word);
        Task StorageTranslatedWordAsync(List<Word> wordList, string path);
        Task StorageTranslatedWordToExcelAsync(List<Word> wordList, string excelName, string? sheet);
    }
}
