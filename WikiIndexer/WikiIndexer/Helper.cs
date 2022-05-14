using System;
using System.Diagnostics;

namespace WikiIndexer
{
    /// <summary>
    ///     ustawienia i metody pomocnicze
    /// </summary>
    public static class Helper
    {
        /// <summary>
        ///     wypisuje czas z stopera
        /// </summary>
        /// <param name="watch">stoper</param>
        /// <returns>napis (h, min, sec) </returns>
        public static string GetHours(ref Stopwatch watch)
        {
            return string.Format("{0:D2} h {1:D2} m {2:D2} sec",
                watch.Elapsed.Hours,
                watch.Elapsed.Minutes,
                watch.Elapsed.Seconds);
        }


        /// <summary>
        ///     wypisuje czas z stopera
        /// </summary>
        /// <param name="watch">stoper</param>
        /// <returns>napis (sec) </returns>
        public static string GetSeconds(ref Stopwatch watch)
        {
            return string.Format("{0:D2} sec", watch.Elapsed.Seconds);
        }


        /// <summary>
        ///     sprawdza czy folder posiada wyłącznie pliki .html
        /// </summary>
        /// <param name="files">lista plików w folderze</param>
        /// <exception cref="Exception">folder nie zawiera wyłącznie plików .html</exception>
        public static void ValidateFolder(ref string[] files)
        {
            foreach (var file in files)
                if (!file.EndsWith(".html"))
                    throw new Exception("FOLDER MUST CONTAIN ONLY .html FILES!");
        }

        #region Settings

        /// <summary>
        ///     ustawienie maksymalnej ilości słów w pliku
        /// </summary>
        public static readonly int MaxWordsInFile = 100;

        /// <summary>
        ///     ustawienie maksymalnej ilości rezultatów na słowo
        /// </summary>
        public static readonly int MaxResultsPerWord = 100;

        public static readonly int MinAmountOfOccurrences = 2;
        public static readonly int MaxAmountOfOccurrences = 50;

        /// <summary>
        ///     w procentach
        ///     im większa tym mniej zostanie skompresowana macierz SVD (100% raw, 0% very compressed)
        ///     oznacza, że waga wartości z wektora musi stanowić conajmniej zadeklarowaną poniżej wartości najwyższej
        /// </summary>
        public static readonly double SvdCompressionRate = 0.99d;

        #endregion
    }
}