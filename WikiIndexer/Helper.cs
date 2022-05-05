using System;
using System.Diagnostics;

namespace WikiIndexer
{
    /// <summary>
    /// ustawienia i metody pomocnicze
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// ustawienie maksymalnej ilości słów w pliku
        /// </summary>
        public static readonly int MaxWordsInFile = 500;
        /// <summary>
        /// ustawienie maksymalnej ilości rezultatów na słowo
        /// </summary>
        public static readonly int MaxResultsPerWord = 100;


        /// <summary>
        /// wypisuje czas z stopera
        /// </summary>
        /// <param name="watch">stoper</param>
        /// <returns>napis (h, min, sec) </returns>
        public static string GetHours(ref Stopwatch watch) =>
            $"{watch.Elapsed.Hours} h {(watch.Elapsed.Hours * 60) - watch.Elapsed.Minutes} min {(watch.Elapsed.Minutes * 60) - watch.Elapsed.Seconds} sec";


        /// <summary>
        /// wypisuje czas z stopera
        /// </summary>
        /// <param name="watch">stoper</param>
        /// <returns>napis (sec) </returns>
        public static string GetSeconds(ref Stopwatch watch) =>
            $"{watch.Elapsed.Seconds} sec";


        /// <summary>
        /// sprawdza czy folder posiada wyłącznie pliki .html
        /// </summary>
        /// <param name="files">lista plików w folderze</param>
        /// <exception cref="Exception">folder nie zawiera wyłącznie plików .html</exception>
        public static void ValidateFolder(ref string[] files)
        {
            foreach (string file in files)
                if (!file.EndsWith(".html"))
                    throw new Exception("FOLDER MUST CONTAIN ONLY .html FILES!");
        }
    }
}
