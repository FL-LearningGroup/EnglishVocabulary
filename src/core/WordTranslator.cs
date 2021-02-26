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
            try
            {
                while (!_webDriver.PageSource.Contains("class=\"qdef\""))
                {
                    pageRefreshCount--;
                    await Task.Delay(1000);
                    _webDriver.Navigate().Refresh();
                    if (pageRefreshCount < 0)
                    {
                        throw new NotFoundException($"Cannot get {word.EnglishLanguage} infomation");
                    }
                }
                IWebElement contentElement = _webDriver.FindElement(By.ClassName("qdef"));
                string txt = contentElement.Text;
                word.Phonetic = _webDriver.PageSource.Contains("class=\"hd_p1_1\"") ? contentElement.FindElement(By.ClassName("hd_p1_1")).Text : null;
                word.ChineseLanguage = _webDriver.PageSource.Contains("ul") ? contentElement.FindElement(By.TagName("ul")).Text : null;
                word.RemoteAudio = _webDriver.PageSource.Contains("class=\"bigaud\"") ? contentElement.FindElement(By.ClassName("bigaud")).GetAttribute("onmouseover") : null;
                word.RemoteAudio = _webDriver.PageSource.Contains(".mp3") ? word.RemoteAudio.Substring(word.RemoteAudio.IndexOf("https"), word.RemoteAudio.IndexOf("mp3") - word.RemoteAudio.IndexOf("https") + 3) : null;
                word.Tense = _webDriver.PageSource.Contains("class=\"hd_if\"") ? contentElement.FindElement(By.ClassName("hd_if")).Text : null;
                return word;
            }
            catch (OpenQA.Selenium.NoSuchElementException ex)
            {
                word.TranslatedMsg = ex.ToString();
                return word;
            }
            catch (NotFoundException ex)
            {
                word.TranslatedMsg = ex.ToString();
                return word;
            }
            catch (Exception ex)
            {
                word.TranslatedMsg = ex.ToString();
                return word;
            }
        }
        public void Dispose()
        {
            _webDriver.Dispose();
        }
    }
}
