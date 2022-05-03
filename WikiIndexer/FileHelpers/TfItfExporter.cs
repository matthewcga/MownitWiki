using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace WikiIndexer.FileHelpers
{
    public static class TfItfExporter
    {
        /// <summary>
        /// struktura bazy danych
        /// macierz tfitf jest glownie pusta wiec zamiast niej mamy taka oto strukture
        /// szukamy po slowie w slowniku, otrzymojemy posortowana liste;
        /// </summary>
        private static Dictionary<string, SortedList<decimal, List<string>>> SearchDb = new Dictionary<string, SortedList<decimal, List<string>>>();

        public static void AddToDict(string word, string file, decimal value)
        {
            if (!SearchDb.ContainsKey(word))
                SearchDb[word] = new SortedList<decimal, List<string>>();

            if (!SearchDb[word].ContainsKey(value))
                SearchDb[word].Add(value, new List<string>());

            SearchDb[word][value].Add(Path.GetFileName(file));
        }

        public static void ExportSearchDb(string dir)
        {
            using (StreamWriter sw = new StreamWriter($"{dir}\\database.json"))
            {
                sw.Write("[\n");
                foreach (KeyValuePair<string, SortedList<decimal, List<string>>> row in SearchDb)
                {
                    List<string> filesToAdd = new List<string>();
                    foreach (KeyValuePair<decimal, List<string>> elem in row.Value)
                        filesToAdd.AddRange(elem.Value);
                    sw.Write(JsonSerializer.Serialize(new KeyValuePair<string, List<string>>(row.Key, filesToAdd)));
                    sw.Write(",\n");
                }
                sw.Write("]");
                sw.Close();
            }
        }
    }
}
