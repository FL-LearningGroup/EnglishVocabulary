using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnglishVocabulary.Core.Models
{
    [Serializable]
    public class Word
    {
        [JsonPropertyName("englishLanguage")]
        public string EnglishLanguage { get; set; }

        [JsonPropertyName("chineseLanguage")]
        public string ChineseLanguage { get; set; }

        [JsonPropertyName("phonetic")]
        public string Phonetic { get; set; }

        [JsonPropertyName("tense")]
        public string Tense { get; set; }

        [JsonPropertyName("remoteAudio")]
        public string RemoteAudio { get; set; }

        [JsonPropertyName("localAudio")]
        public string LocalAudio { get; set; }

        [JsonPropertyName("translatedMsg")]
        public string TranslatedMsg { get; set; }
    }
}
