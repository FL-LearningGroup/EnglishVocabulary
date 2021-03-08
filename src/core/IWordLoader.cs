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
        Task<IEnumerable<string>> LoadWordFromFileAsync(string path);
        IEnumerable<string> LoadWordFromExcelAsync(string path, string sheetName = null);
        Task GetWordMp3FileAsync(Word word);
        Task StorageTranslatedWordAsync(List<Word> wordList, string path);
        Task StorageTranslatedWordAsync(List<Word> wordList, int wordRange, string folder, string baseFileName);

        Task StorageTranslatedWordToExcelAsync(List<Word> wordList, int wordRange, string excelPath, string sheet);

        Task StorageTranslatedWordToExcelAsync(List<Word> wordList, string excelPath, string? sheet);
    }
}
