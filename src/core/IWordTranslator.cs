using EnglishVocabulary.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVocabulary.Core
{
    public interface IWordTranslator
    {
        Task<Word> TranslateAsync(string sorceWord);
    }
}
