using Porter2StemmerStandard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace WikiSearcher.Database
{
    public static class DbManager
    {
        private static readonly EnglishPorter2Stemmer Stemmer = new EnglishPorter2Stemmer();
        public static Dictionary<string, List<string>> DataBase;
        public static Dictionary<string, string> LinksDataBase;


        public static List<string> AskDb(string search)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Dictionary<string, int> resultsRanker = new Dictionary<string, int>();
            List<string> keywords = StemSearch(search), results;

            foreach (string keyword in keywords)
            {
                SingleWordQuerry(keyword, ref resultsRanker);
            }

            results = resultsRanker.OrderBy(x => x.Value).Select(x => x.Key).ToList();
            sw.Stop();
            Program.ChangeStatus($"found {results.Count} results in {sw.Elapsed.TotalMilliseconds}ms");
            return results;
        }


        private static void SingleWordQuerry(string keyword, ref Dictionary<string, int> resultsRanker)
        {
            List<string> resultsForKey = DataBase.ContainsKey(keyword) ? DataBase[keyword] : new List<string>();
            foreach (string file in resultsForKey)
            {
                if (!resultsRanker.ContainsKey(file)) resultsRanker.Add(file, 0);
                resultsRanker[file]++;
            }
        }


        private static List<string> StemSearch(string search)
        {
            search = Regex.Replace(search, @"[^a-zA-Z]", " ");
            string[] splited = search.Split(new char[] { ' ', ',', ';', '.', '/', '\"', '[', ']', '!' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> results = new List<string>();

            foreach (string elem in splited)
            {
                string word = elem.Trim();
                word = word.ToLower();
                try { results.Add(Stemmer.Stem(word).Value); }
                catch { Console.WriteLine($"\nWARNING: stemmer failed on word: '{word}'\n"); } // stemmer potrafi wywalić się dla dziwnych słów (jakiś niemieckich np.)
            }
            return results;
        }


        public static void GetDbFromFile(string dir)
        {
            string path = $"{dir}\\database.json", linksPath = $"{dir}\\links.json";

            if (!ValidateFolder(ref path, ref linksPath)) return;

            Stopwatch sw = Stopwatch.StartNew();
            JsonSerializerOptions options = new JsonSerializerOptions { AllowTrailingCommas = true };
            DataBase = new Dictionary<string, List<string>>();
            LinksDataBase = new Dictionary<string, string>();

            using (StreamReader sr = new StreamReader(path))
            {
                List<KeyValuePair<string, List<string>>> json = JsonSerializer
                    .Deserialize<List<KeyValuePair<string, List<string>>>>(sr.ReadToEnd(), options);

                foreach (KeyValuePair<string, List<string>> entry in json)
                    DataBase.Add(entry.Key, entry.Value);
            }

            using (StreamReader sr = new StreamReader(linksPath))
            {
                List<KeyValuePair<string, string>> json = JsonSerializer
                    .Deserialize<List<KeyValuePair<string, string>>>(sr.ReadToEnd(), options);
                foreach (KeyValuePair<string, string> entry in json)
                    LinksDataBase.Add(entry.Key, entry.Value);
            }

            sw.Stop();
            Program.ChangeStatus($"imported indexes and links to {LinksDataBase.Count} articles in {sw.Elapsed.TotalMilliseconds} ms");
            Program.SetSearchElements(true);
        }


        private static bool ValidateFolder(ref string path, ref string linksPath)
        {
            if (!File.Exists(path) || !File.Exists(linksPath))
            {
                Program.ChangeStatus($"no database in directory");
                Program.SetSearchElements(false);
                Program.SetFileElements(false);
                return false;
            }
            else return true;
        }
    }
}
