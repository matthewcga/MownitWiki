using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiIndexer
{
    public static class Helper
    {
        public static readonly int MaxWordsInFile = 500;

        public static string GetHours(ref Stopwatch watch) =>
            $"{watch.Elapsed.Hours} h {watch.Elapsed.Minutes - (watch.Elapsed.Hours * 60)} min {watch.Elapsed.Seconds - (watch.Elapsed.Minutes * 60)} sec";

        public static string GetSeconds(ref Stopwatch watch) =>
            $"{watch.Elapsed.Seconds} sec";

        public static void ValidateFolder(ref string[] files)
        {
            foreach (string file in files)
                if (!file.EndsWith(".html"))
                    throw new Exception("FOLDER MUST CONTAIN ONLY .html FILES!");
        }
    }
}
