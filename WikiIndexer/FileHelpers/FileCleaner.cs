using HtmlAgilityPack;
using Porter2StemmerStandard;
using StopWord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WikiIndexer.FileHelpers
{
    /// <summary>
    /// zawiera metody do: czyszczenia, tokenizacji i stemowania plików
    /// </summary>
    public static class FileCleaner
    {
        /// <summary>
        /// Stopwords, czyli np. 'a', 'the' i inne niepotrzebne
        /// </summary>
        private static readonly HashSet<string> StopWordsSet = new HashSet<string>(StopWords.GetStopWords("en").ToList());
        /// <summary>
        /// stemmer, sprowadza słowo do korzenia, np: stopped -> stop?
        /// </summary>
        private static readonly EnglishPorter2Stemmer Stemmer = new EnglishPorter2Stemmer();


        /// <summary>
        /// usuwa tagi html, linki i inne rzeczy zostawiając wyłącznie słowa
        /// </summary>
        /// <param name="file"></param>
        public static void CleanFile(ref string file)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(file);
            file = htmlDoc.DocumentNode.InnerText;
            file = Regex.Replace(file, @"[^a-zA-Z]", " ");
        }


        /// <summary>
        /// Tokenizacja (rozbicie na słowa?) i Stemowanie (sprowadzanie do korzenia)
        /// </summary>
        /// <param name="file">treść pliku</param>
        /// <param name="words">lista do której zwracane są stemowane słowa</param>
        public static void StemFile(ref string file, ref List<string> words)
        {
            string[] splited = file.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string elem in splited)
            {
                string word = elem.Trim();
                word = word.ToLower();
                try { if (!StopWordsSet.Contains(word)) words.Add(Stemmer.Stem(word).Value); }
                catch { Console.WriteLine($"\nWARNING: stemmer failed on word: '{word}'\n"); } // stemmer potrafi wywalić się dla dziwnych słów (jakiś niemieckich np.)
                if (words.Count >= Helper.MaxWordsInFile) return;
            }

        }
    }
}
