using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVocabulary.Core.Models
{
    public class Word
    {
        public string EnglishLanguage { get; set; }
        public string ChineseLanguage { get; set; }
        public string Phonetic { get; set; }
        public string Tense { get; set; }
        public string RemoteAudio { get; set; }
        public string LocalAudio { get; set; }
    }
}
