using System;
using System.Diagnostics;

namespace WikiIndexer
{
    public static class Helper
    {
        public static string GetHours(ref Stopwatch watch)
        {
            return string.Format("{0:D2} h {1:D2} m {2:D2} sec",
                watch.Elapsed.Hours,
                watch.Elapsed.Minutes,
                watch.Elapsed.Seconds);
        }


        public static string GetSeconds(ref Stopwatch watch)
        {
            return string.Format("{0:D2} sec", watch.Elapsed.Seconds);
        }


        public static void ValidateFolder(ref string[] files)
        {
            foreach (var file in files)
                if (!file.EndsWith(".html"))
                    throw new Exception("FOLDER MUST CONTAIN ONLY .html FILES!");
        }

        #region Settings
        public static readonly int MaxWordsInFile = 100;
        public static readonly int MaxResultsPerWord = 100;
        public static readonly int MinAmountOfOccurrences = 2;
        public static readonly int MaxAmountOfOccurrences = 50;
        public static readonly double SvdCompressionRate = 0.99d;
        #endregion
    }
}