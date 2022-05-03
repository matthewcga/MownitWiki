using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WikiIndexer.Dictionary
{
    public static class StemmedDictionary
    {
        private static readonly HashSet<string> _dict = new HashSet<string>();
        private static readonly int _dictSize;

        public static HashSet<string> StemDict { get => _dict; }
        public static int StemDictSize { get => _dictSize; }

        static StemmedDictionary()
        {
            Console.WriteLine("\ninitializing stemmed dictionary\nplease wait...");

            Stopwatch watch = Stopwatch.StartNew();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Dictionary\stemmedDictionary.txt");
            foreach (string word in File.ReadLines(path))
                _dict.Add(word);

            _dictSize = _dict.Count();
            watch.Stop();

            Console.WriteLine($"\nfinished stemming dictionary in {Helper.GetSeconds(ref watch)}!\nadded {_dictSize} words");
        }

        public static void GetDict<T>(ref Dictionary<string, T> stemDict, T def)
        { stemDict = StemDict.ToDictionary(x => x, x => def); }
    }
}
