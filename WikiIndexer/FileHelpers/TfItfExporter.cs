using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace WikiIndexer.FileHelpers
{
    /// <summary>
    /// Klasa która obługuje kompresje macierzy TF-ITF i jej eskport do .json
    /// </summary>
    public static class TfItfExporter
    {
        /// <summary>
        /// struktura bazy danych
        /// macierz tfitf jest glownie pusta wiec zamiast niej mamy taka oto strukture
        /// szukamy po slowie w slowniku, otrzymojemy posortowana liste;
        /// </summary>
        private static Dictionary<string, SortedDictionary<decimal, List<string>>> SearchDb = new Dictionary<string, SortedDictionary<decimal, List<string>>>();


        /// <summary>
        /// Dodawanie słowa z słownika TF do bazy SearchDb
        /// </summary>
        /// <param name="word">dodawane słowo</param>
        /// <param name="file">plik z którego pochodzi</param>
        /// <param name="value">jego istotność TF-ITF</param>
        public static void AddToDict(string word, string file, decimal value)
        {
            if (!SearchDb.ContainsKey(word)) // sprawdza czy słowo już jest w słowniku
                SearchDb[word] = new SortedDictionary<decimal, List<string>>();

            if (!SearchDb[word].ContainsKey(value)) // sprawdza czy wartośc value już jest dla danego słowa w słowniku
                SearchDb[word].Add(value, new List<string>());

            SearchDb[word][value].Add(Path.GetFileName(file));
        }


        /// <summary>
        /// eksport SearchDb do .json
        /// </summary>
        public static void ExportSearchDb()
        {
            using (StreamWriter sw = new StreamWriter($"{Program.Dir}\\database.json"))
            {
                sw.Write("[\n");
                foreach (KeyValuePair<string, SortedDictionary<decimal, List<string>>> row in SearchDb)
                {
                    List<string> filesToAdd = new List<string>(Helper.MaxResultsPerWord);
                    foreach (KeyValuePair<decimal, List<string>> elem in row.Value)
                        SafeAddRange(ref filesToAdd, elem.Value);
                    sw.Write(JsonSerializer.Serialize(new KeyValuePair<string, List<string>>(row.Key, filesToAdd)));
                    sw.Write(",\n");
                }
                sw.Write("]");
                sw.Close();
            }
        }


        /// <summary>
        /// ucina nadmiar przy łączeniu list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        private static void SafeAddRange<T>(ref List<T> l1, List<T> l2)
        {
            if (l1.Count + l2.Count <= l1.Capacity) l1.AddRange(l2);
            else l1.AddRange(l2.Take(l1.Capacity - l1.Count).ToList());
        }
    }
}
