using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;
using EnglishVocabulary.Core;
using EnglishVocabulary.Core.Models;
using System.Linq;
using System.ComponentModel;

namespace EnglishVocabulary.Pwsh
{
    [Cmdlet("Translate", "EnglishWord")]
    public class TranslateEnglishWord : Cmdlet
    {
        private string _path;
        [Parameter(Mandatory = true, HelpMessage = "The file contain words.")]
        public string Path
        {
            get { return _path; }
            set
            {
                FileInfo file = new FileInfo(value);
                if (!file.Exists) throw new FileNotFoundException($"The file {value} not been found.");
                _path = value;
            }
        }

        [Parameter(HelpMessage = "Whether download word mp3. If ture that download.")]
        public bool IsAudio = true;

        [Parameter(HelpMessage = "Storage translated result to xlsx file.")]
        public bool IsExcel = true;

        [Parameter(Mandatory = true, HelpMessage = "Storage word file name")]
        public string Name { get; set; }

        [Parameter(HelpMessage = "If word count greater and equal split then splict word into differents file.")]
        [DefaultValue(50)]
        public int Split { get; set; }

        private string _storageFolder;
        [Parameter(Mandatory = true, HelpMessage = "the word mp3 storge path.")]
        public string StorageFolder
        {
            get { return _storageFolder; }
            set
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(value);
                if (!directoryInfo.Exists) directoryInfo.Create() ;
                _storageFolder = value;
            }
        }
        protected override void BeginProcessing()
        {

        }

        // Override the ProcessRecord method to process
        // the supplied user name and write out a
        // greeting to the user by calling the WriteObject
        // method.
        protected override void ProcessRecord()
        {
            ProcessRecordAsync().Wait();
        }

        protected async Task ProcessRecordAsync()
        {
            try
            {
                List<Word> wordList = new List<Word>();
                IEnumerable<string> sourceWords;
                IWordLoader wordLoader = new WordLoader();
                IWordTranslator wordTranslator = new WordTranslator();
                // Load word list from file
                WriteObject("Load word from the word file.");
                string fileType = _path.Substring(_path.LastIndexOf('.') + 1);
                if (fileType == "txt")
                {
                    sourceWords = await wordLoader.LoadWordFromFileAsync(_path);
                }
                else if (fileType == "xlsx")
                {
                    sourceWords = wordLoader.LoadWordFromExcelAsync(_path);
                }
                else
                {
                    throw new Exception($"{_path} is type invalid), Supported type includes xlsx, txt.");
                }

                var sortedSourceWord = from word in sourceWords
                                       orderby word ascending
                                       select word;
                // Translate word
                WriteObject("Translate word...");
                foreach (string word in sortedSourceWord)
                {
                    wordList.Add(await wordTranslator.TranslateAsync(word));
                }
                wordList.ForEach(item => item.LocalAudio = _storageFolder + $"\\audio\\" + item.EnglishLanguage + ".mp3");
                // Storage translated result to file.
                await wordLoader.StorageTranslatedWordAsync(wordList, Split, _storageFolder, Name);

                // Storage transfated result to excel file.
                if (IsExcel)
                {
                    await wordLoader.StorageTranslatedWordToExcelAsync(wordList, Split, _storageFolder + $@"\{Name}.xlsx", Name);
                }
                // Get andt storge mp3 file.

                if (IsAudio)
                {
                    WriteObject("Download mp3 file from web site.");
                    foreach (var item in wordList)
                    {
                        await wordLoader.GetWordMp3FileAsync(item);
                    }
                }
                WriteObject("Translated end");
            }
            catch
            {
                throw;
            }
        }
        protected override void EndProcessing()
        {
        }
    }
}
