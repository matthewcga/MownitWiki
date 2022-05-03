using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using WikiIndexer.Dictionary;
using DecimalMath;

namespace WikiIndexer.FileHelpers
{
    public static class TfItfComputer
    {
        private static void ComputeTF(ref List<string> words, ref Dictionary<string, decimal> tf)
        {
            foreach (string word in words)
                if (tf.ContainsKey(word)) tf[word]++;
                else tf.Add(word, 1m);
        }

        private static void EditITF(ref Dictionary<string, decimal> itf, ref Dictionary<string, decimal> tf)
        {
            foreach (string word in tf.Keys.ToList())
            {
                if (!itf.ContainsKey(word))
                    itf.Add(word, 0m);
                itf[word] += tf[word];
            }
            //if (!itf.ContainsKey(word))
            //    //Console.Write($"\nWARNING: unknown stemmed word: '{word}'\n");
            //    tf.Remove(word);
            //else
            //    //Console.Write($"\nDEBUG: known stemmed word: '{word}'\n");
            //    itf[word] += tf[word];
        }

        private static void LogITF(ref Dictionary<string, decimal> itf, int filesCount)
        {
            foreach (string word in itf.Keys.ToList())
                itf[word] = itf[word] > 0m ? DecimalEx.Log(filesCount / itf[word]) : 0m;
        }

        private static void ComputeTFITF(ref Dictionary<string, decimal> itf, ref Dictionary<string, decimal>[] tf, ref string[] files)
        {
            int filesCount = files.Length, itfSize = itf.Count();
            for (int i = 0; i < filesCount; i++)
            {
                foreach (string word in itf.Keys.ToList())
                {
                    if (tf[i].ContainsKey(word))
                    {
                        tf[i][word] = itf[word] * (tf[i][word] / itfSize);
                        TfItfExporter.AddToDict(word, files[i], tf[i][word]);
                    }
                }
                Console.Write($"\r\tTF dictionary: {i + 1}/{filesCount}");
            }
        }

        public static void ComputeTFITF(ref string dir)
        {
            Console.Write($"\nselected directory:\n{dir}\n\n1/4 starting computing TFITF\n");

            string[] files = Directory.GetFiles($"{dir}\\downloads");
            int filesCount = files.Length, i = 0;
            Helper.ValidateFolder(ref files);

            Dictionary<string, decimal> itf = new Dictionary<string, decimal>();
            Dictionary<string, decimal>[] tf = new Dictionary<string, decimal>[filesCount];
            //StemmedDictionary.GetDict(ref itf, 0m);

            Stopwatch watch = Stopwatch.StartNew();
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                Console.Write($"\r\t {fileName}: {i + 1}/{filesCount}, step 1/5 reading");
                string content = File.ReadAllText(file);

                Console.Write($"\r\t {fileName}: {i + 1}/{filesCount}, step 2/5 cleaning");
                FileCleaner.CleanFile(ref content);

                Console.Write($"\r\t {fileName}: {i + 1}/{filesCount}, step 3/5 stemming");
                List<string> words = new List<string>();
                FileCleaner.StemFile(ref content, ref words);

                Console.Write($"\r\t {fileName}: {i + 1}/{filesCount}, step 4/5 creating TF dictionary");
                tf[i] = new Dictionary<string, decimal>();
                ComputeTF(ref words, ref tf[i]); // raw tf (nie podzielone przez liczbe slow) (tylko wystapienia)

                Console.Write($"\r\t {fileName}: {i + 1}/{filesCount}, step 5/5 altering ITF dictionary\n");
                EditITF(ref itf, ref tf[i]);

                i++;
            }
            watch.Stop();

            Console.Write($"\nfinished parsing documents in {Helper.GetHours(ref watch)}!\n2/4 modifing ITF to its final form\n");

            watch.Restart();
            LogITF(ref itf, filesCount);
            watch.Stop();

            Console.Write($"\nfinished modifing ITF in {Helper.GetHours(ref watch)}!\n3/4 converting TF[] to TF-ITF matrix\nconverting matrix to database\n");

            watch.Restart();
            ComputeTFITF(ref itf, ref tf, ref files);
            watch.Stop();

            Console.Write($"\n\nfinished computing TFIDF in {Helper.GetHours(ref watch)}!\n4/4 exporting database to .json\n");

            watch.Restart();
            TfItfExporter.ExportSearchDb(dir);
            watch.Stop();

            Console.Write($"\nfinished exporting TFIDF to external .json in {Helper.GetHours(ref watch)}!\n");
        }
    }
}
