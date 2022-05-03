using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StopWord;
using Porter2StemmerStandard;

namespace WikiIndexer.FileHelpers
{
    public static class FileCleaner
    {
        private static readonly HashSet<string> StopWordsSet = new HashSet<string>(StopWords.GetStopWords("en").ToList());
        private static readonly EnglishPorter2Stemmer Stemmer = new EnglishPorter2Stemmer();

        public static void CleanFile(ref string file)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(file);
            file = htmlDoc.DocumentNode.InnerText;
            file = Regex.Replace(file, @"\W+|\d+|\e+", " ");
            file = Regex.Replace(file, @"http.?[^\ ]*\ ", " ");
        }

        public static void StemFile(ref string file, ref List<string> words)
        {
            string[] splited = file.Split(new char[] { ' ', ',', ';', '.', '/', '\"', '[', ']', '!' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string elem in splited)
            {
                string word = elem.Trim();
                word = word.ToLower();
                if (!StopWordsSet.Contains(word))
                    words.Add(Stemmer.Stem(word).Value);
                if (words.Count >= Helper.MaxWordsInFile)
                    return;
            }

        }
    }
}
