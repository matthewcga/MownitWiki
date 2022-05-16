using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Porter2StemmerStandard;
using StopWord;

namespace WikiIndexer
{
    public static class Cleaner
    {
        private static readonly HashSet<string> StopWordsSet =
            new HashSet<string>(StopWords.GetStopWords("en").ToList());

        private static readonly EnglishPorter2Stemmer Stemmer = new EnglishPorter2Stemmer();


        public static void ParseFiles(ref string[] files, ref Dictionary<string, double>[] tf,
            ref Dictionary<string, double> itf)
        {
            var i = 0;
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                CleanFile(ref content);
                var words = new List<string>();
                StemFile(ref content, ref words);
                tf[i] = new Dictionary<string, double>();
                ComputeTf(ref words, ref tf[i], ref itf);
                i++;
                Console.Write($"\r\t{i.ToString().PadLeft(Program.Width, ' ')}/{Program.FilesCount}");
            }
        }


        public static void ComputeTf(ref List<string> words, ref Dictionary<string, double> tf,
            ref Dictionary<string, double> itf)
        {
            foreach (var word in words)
            {
                if (tf.ContainsKey(word)) tf[word]++;
                else tf.Add(word, 1d);

                if (itf.ContainsKey(word)) itf[word]++;
                else itf.Add(word, 1d);
            }
        }


        public static void CleanFile(ref string file)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(file);
            file = htmlDoc.DocumentNode.InnerText;
            file = Regex.Replace(file, @"[^a-zA-Z]", " ");
        }


        public static void StemFile(ref string file, ref List<string> words)
        {
            var splited = file.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var elem in splited)
            {
                var word = elem.Trim();
                word = word.ToLower();
                try
                {
                    if (!StopWordsSet.Contains(word)) words.Add(Stemmer.Stem(word).Value);
                }
                catch
                {
                    Console.WriteLine($"\nWARNING: stemmer failed on word: '{word}'\n");
                } // stemmer potrafi wywalić się dla dziwnych słów (jakiś niemieckich np.)

                if (words.Count >= Helper.MaxWordsInFile) return;
            }
        }
    }
}