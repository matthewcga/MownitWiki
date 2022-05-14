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
    /// <summary>
    ///     zawiera metody do: czyszczenia, tokenizacji i stemowania plików
    /// </summary>
    public static class Cleaner
    {
        /// <summary>
        ///     Stopwords, czyli np. 'a', 'the' i inne niepotrzebne
        /// </summary>
        private static readonly HashSet<string> StopWordsSet =
            new HashSet<string>(StopWords.GetStopWords("en").ToList());

        /// <summary>
        ///     stemmer, sprowadza słowo do korzenia, np: stopped -> stop?
        /// </summary>
        private static readonly EnglishPorter2Stemmer Stemmer = new EnglishPorter2Stemmer();


        /// <summary>
        ///     metoda odpowiada za wyczyszczenie i ostemowanie wszystkich plików
        /// </summary>
        /// <param name="files">ścieżki do plików</param>
        /// <param name="tf">słowniki TF</param>
        /// <param name="itf">słownik ITF</param>
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
                Console.Write($"\r\t file: {i.ToString().PadLeft(Program.Width, ' ')}/{Program.FilesCount}");
            }
        }


        /// <summary>
        ///     zliczanie słów (term frequency) dla danego pliku
        /// </summary>
        /// <param name="words">lista słów z pliku (już po stemowaniu)</param>
        /// <param name="tf">uzupełniany słownik</param>
        /// /// <param name="itf">słownik itf</param>
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


        /// <summary>
        ///     usuwa tagi html, linki i inne rzeczy zostawiając wyłącznie słowa
        /// </summary>
        /// <param name="file"></param>
        public static void CleanFile(ref string file)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(file);
            file = htmlDoc.DocumentNode.InnerText;
            file = Regex.Replace(file, @"[^a-zA-Z]", " ");
        }


        /// <summary>
        ///     Tokenizacja (rozbicie na słowa?) i Stemowanie (sprowadzanie do korzenia)
        /// </summary>
        /// <param name="file">treść pliku</param>
        /// <param name="words">lista do której zwracane są stemowane słowa</param>
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