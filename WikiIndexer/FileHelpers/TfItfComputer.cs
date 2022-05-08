using DecimalMath;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace WikiIndexer.FileHelpers
{
    /// <summary>
    /// klasa obsługująca kalkulachje macierzy TF-ITF oraz wszelkie etapy do niej prowadzące
    /// </summary>
    public static class TfItfComputer
    {
        /// <summary>
        /// słownik ITF (inverse term frequency), zlicza ilość słów w wszystkich plikach (wartość pól opisana w metodzie LogITF)
        /// </summary>
        private static Dictionary<string, decimal> ITF = new Dictionary<string, decimal>();
        /// <summary>
        /// tablica słowników TF (term frequency), zlicza ilość słów w pojedyńczym pliku (wartość pól opisana w metodzie FillTFITF)
        /// </summary>
        private static Dictionary<string, decimal>[] TF;
        /// <summary>
        /// liczba plików
        /// </summary>
        private static int FilesCount, Width;


        /// <summary>
        /// metoda odpowiada za wyczyszczenie i ostemowanie wszystkich plików
        /// </summary>
        /// <param name="files">ścieżki do plików</param>
        private static void ParseFiles(ref string[] files)
        {
            int i = 0;
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string content = File.ReadAllText(file);
                FileCleaner.CleanFile(ref content);
                List<string> words = new List<string>();
                FileCleaner.StemFile(ref content, ref words);
                TF[i] = new Dictionary<string, decimal>();
                ComputeTF(ref words, ref TF[i]);

                i++;
                Console.Write($"\r\t file: {i.ToString().PadLeft(Width, ' ')}/{FilesCount}");
            }
        }


        /// <summary>
        /// zliczanie słów (term frequency) dla danego pliku
        /// </summary>
        /// <param name="words">lista słów z pliku (już po stemowaniu)</param>
        /// <param name="tf">uzupełniany słownik</param>
        private static void ComputeTF(ref List<string> words, ref Dictionary<string, decimal> tf)
        {
            foreach (string word in words)
            {
                if (tf.ContainsKey(word)) tf[word]++;
                else tf.Add(word, 1m);

                if (ITF.ContainsKey(word)) ITF[word]++;
                else ITF.Add(word, 1m);
            }

        }


        /// <summary>
        /// metoda oblicza ostateczną wartośc słowa w słowniku ITF równą ln( ilość plików / liczba wystąpień słowa)
        /// </summary>
        private static void LogITF()
        {
            foreach (string word in ITF.Keys.ToList())
                ITF[word] = ITF[word] > 0m ? DecimalEx.Log(FilesCount / ITF[word]) : 0m;
        }


        /// <summary>
        /// metoda oblicza TF-ITF dla pliku, gdzie TF-ITF oznacza dla słowa (jego wartość TF z pliku) * (jego wartość z globalnego słownika ITF)
        /// </summary>
        /// <param name="files">pliki .html</param>
        private static void FillTFITF(ref string[] files)
        {
            int filesCount = files.Length, itfSize = ITF.Count();
            for (int i = 0; i < filesCount; i++)
            {
                foreach (string word in ITF.Keys.ToList())
                {
                    if (TF[i].ContainsKey(word))
                    {
                        TF[i][word] = ITF[word] * (TF[i][word] / itfSize);
                        TfItfExporter.AddToDict(word, files[i], TF[i][word]);
                    }
                }
                Console.Write($"\r\tTF dictionary: {(i + 1).ToString().PadLeft(Width, ' ')}/{filesCount}");
            }
        }


        /// <summary>
        /// metoda matka obliczająca TF-ITF i wywołująca metody któe wykonują pomniejsze etapy
        /// </summary>
        public static void ComputeTFITF()
        {
            string[] files = Directory.GetFiles($"{Program.Dir}\\downloads");
            FilesCount = files.Length;
            Width = FilesCount.ToString().Length;
            Helper.ValidateFolder(ref files);
            TF = new Dictionary<string, decimal>[FilesCount];

            Console.Write($"{Program.Dir}\n\n1/4 cleaning, stemming, computing TF[] ...\n");

            Stopwatch watch = Stopwatch.StartNew();
            ParseFiles(ref files);
            watch.Stop();

            Console.Write($"\nfinished in {Helper.GetHours(ref watch)}!\n\n2/4 modifing ITF ...\n");

            LogITF();

            Console.Write($"finished!\n\n3/4 converting TF[] to TF-ITF ...\n");

            watch.Restart();
            FillTFITF(ref files);
            watch.Stop();

            Console.Write($"\nfinished in {Helper.GetHours(ref watch)}!\n\n4/4 exporting indexes to .json ...\n");

            watch.Restart();
            TfItfExporter.ExportSearchDb();
            watch.Stop();

            Console.Write($"finished in {Helper.GetHours(ref watch)}!\n");
        }
    }
}
