using EnglishVocabulary.Core.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EnglishVocabulary.Core
{
    public class WordTranslator: IWordTranslator, IDisposable
    {
        private IWebDriver _webDriver;
        public WordTranslator()
        {
            _webDriver = new ChromeDriver();
        }
        public async Task<Word> TranslateAsync(string sorceWord)
        {
            Word word = new Word();
            word.EnglishLanguage = sorceWord;
            string queryUrl = MicrosoftBingDictionary.DictSearch + sorceWord;
            int pageRefreshCount = 5;
            _webDriver.Url = queryUrl;
            // Check chrome whether successfully loaded the word page.
            while (!_webDriver.PageSource.Contains("class=\"qdef\""))
            {
                pageRefreshCount--;
                await Task.Delay(1000);
                _webDriver.Navigate().Refresh();
                if (pageRefreshCount < 0)
                {
                    break;
                }
            }
            try
            {
                IWebElement contentElement = _webDriver.FindElement(By.ClassName("qdef"));
                string txt = contentElement.Text;
                word.Phonetic = contentElement.FindElement(By.ClassName("hd_p1_1")).Text;
                word.ChineseLanguage =  contentElement.FindElement(By.TagName("ul")).Text;
                word.RemoteAudio =  contentElement.FindElement(By.ClassName("bigaud")).GetAttribute("onmouseover");
                word.RemoteAudio = word.RemoteAudio.Substring(word.RemoteAudio.IndexOf("https"), word.RemoteAudio.IndexOf("mp3") - word.RemoteAudio.IndexOf("https") + 3);
                word.Tense = contentElement.FindElement(By.ClassName("hd_if")).Text;
            }
            catch (OpenQA.Selenium.NoSuchElementException ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

            return word;
        }
        public void Dispose()
        {
            _webDriver.Dispose();
        }
    }
}
