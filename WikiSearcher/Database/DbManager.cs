using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Diagnostics;
using Porter2StemmerStandard;

namespace WikiSearcher.Database
{
    public static class DbManager
    {
        private static readonly EnglishPorter2Stemmer Stemmer = new EnglishPorter2Stemmer();
        private static Dictionary<string, List<string>> _db;
        private static Dictionary<string, string> _linksDb;

        // to do: akceptowanie kilku slow

        public static string GetLink(string file)
        {
            return _linksDb[file];
        }

        public static List<string> FileQuery(string word)
        {
            Stopwatch sw = Stopwatch.StartNew();
            word = StemQuestion(word);
            List<string> result = _db.ContainsKey(word) ? _db[word] : new List<string>();
            sw.Stop();
            Program.ChangeStatus($"found {result.Count} results in {sw.Elapsed.TotalMilliseconds}ms");
            return result;
        }

        public static string StemQuestion(string word)
        {
            //string[] splited = file.Split(new char[] { ' ', ',', ';', '.', '/', '\"', '[', ']', '!' }, StringSplitOptions.RemoveEmptyEntries);
            //foreach (string word in splited)
            //{
            //    word.Trim();
            //    if (!StopWordsSet.Contains(word))
            //        words.Add(Stemmer.Stem(word).Value);
            //    if (words.Count >= Helper.MaxWordsInFile)
            //        return;
            //}
            word = word.Trim();
            word = word.ToLower();
            return Stemmer.Stem(word).Value;
        }

        public static void GetDbFromFile(string dir)
        {
            string path = $"{dir}\\database.json", linksPath = $"{dir}\\links.json";
            JsonSerializerOptions options = new JsonSerializerOptions { AllowTrailingCommas = true };

            if (!File.Exists(path) || !File.Exists(linksPath))
            {
                Program.ChangeStatus($"no database in directory");
                Program.SetSearchElements(false);
                Program.SetFileElements(false);
                return;
            }

            Program.ChangeStatus($"importing database form: '{path}'");
            Stopwatch sw = Stopwatch.StartNew();

            _db = new Dictionary<string, List<string>>();
            _linksDb = new Dictionary<string, string>();

            using (StreamReader sr = new StreamReader(path))
            {
                List<KeyValuePair<string, List<string>>> json = JsonSerializer
                    .Deserialize<List<KeyValuePair<string, List<string>>>>(sr.ReadToEnd(), options);

                foreach (KeyValuePair<string, List<string>> entry in json)
                    _db.Add(entry.Key, entry.Value);
            }

            using (StreamReader sr = new StreamReader(linksPath))
            {
                List<KeyValuePair<string, string>> json = JsonSerializer
                    .Deserialize<List<KeyValuePair<string, string>>>(sr.ReadToEnd(), options);

                foreach (KeyValuePair<string, string> entry in json)
                    _linksDb.Add(entry.Key, entry.Value);
            }

            sw.Stop();
            Program.ChangeStatus($"succesfully imported indexes and links to {_linksDb.Count} articles in {sw.Elapsed.TotalMilliseconds} ms");
            Program.SetSearchElements(true);
        }
    }
}
