using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using MathNet.Numerics.LinearAlgebra.Double;

namespace WikiIndexer
{
    public static class Exporter
    {
        private static void AddToDict(ref Dictionary<string, SortedDictionary<double, List<string>>> searchDb,
            string word, string file, double value)
        {
            if (!searchDb.ContainsKey(word)) // sprawdza czy słowo już jest w słowniku
                searchDb[word] = new SortedDictionary<double, List<string>>();

            if (!searchDb[word].ContainsKey(value)) // sprawdza czy wartośc value już jest dla danego słowa w słowniku
                searchDb[word].Add(value, new List<string>());

            searchDb[word][value].Add(Path.GetFileName(file));
        }


        private static void ExportSearchDb(ref Dictionary<string, SortedDictionary<double, List<string>>> searchDb, bool withSvd)
        {
            string name = withSvd ? "database" : "databaseSVD";
            using (var sw = new StreamWriter($"{Program.Dir}\\{name}.json"))
            {
                sw.Write("[\n");
                foreach (var row in searchDb)
                {
                    var filesToAdd = new List<string>(Helper.MaxResultsPerWord);
                    foreach (var elem in row.Value)
                        SafeAddRange(ref filesToAdd, elem.Value);
                    sw.Write(JsonSerializer.Serialize(new KeyValuePair<string, List<string>>(row.Key, filesToAdd)));
                    sw.Write(",\n");
                }

                sw.Write("]");
                sw.Close();
            }
        }


        private static void SafeAddRange<T>(ref List<T> l1, List<T> l2)
        {
            if (l1.Count + l2.Count <= l1.Capacity) l1.AddRange(l2);
            else l1.AddRange(l2.Take(l1.Capacity - l1.Count).ToList());
        }


        public static void ExportMatrix(ref SparseMatrix matrix, ref string[] files, bool withSvd)
        {
            var searchDb = new Dictionary<string, SortedDictionary<double, List<string>>>();

            Console.Write("\n\nbuilding database ...\n");

            var watch = Stopwatch.StartNew();
            for (var col = 0; col < Computer.BagOfWords.Length; col++)
                for (var row = 0; row < Program.FilesCount; row++)
                    if (matrix[row, col] > 0)
                        AddToDict(ref searchDb, Computer.BagOfWords[col], files[row], matrix[row, col]);
            watch.Stop();

            Console.Write($"finished in {Helper.GetHours(ref watch)}!\n\nexporting to .json ...\n");

            watch.Restart();
            ExportSearchDb(ref searchDb, withSvd);
            watch.Stop();

            Console.Write($"finished in {Helper.GetHours(ref watch)}!");
        }
    }
}