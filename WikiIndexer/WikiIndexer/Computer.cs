using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace WikiIndexer
{
    /// <summary>
    ///     klasa obsługująca kalkulachje macierzy TF-ITF oraz wszelkie etapy do niej prowadzące
    /// </summary>
    public static class Computer
    {
        public static string[] BagOfWords;


        /// <summary>
        ///     metoda oblicza ostateczną wartośc słowa w słowniku ITF równą ln( ilość plików / liczba wystąpień słowa)
        /// </summary>
        public static void LogAndCleanItf(ref Dictionary<string, double> itf)
        {
            foreach (var word in itf.Keys.ToList())
                if (itf[word] >= Helper.MinAmountOfOccurrences && itf[word] <= Helper.MaxAmountOfOccurrences)
                    itf[word] = itf[word] > 0d ? Math.Log(Program.FilesCount / itf[word]) : 0d;
                else
                    itf.Remove(word);
        }


        public static void BuildNormalizedMatrix(ref string[] files, ref Dictionary<string, double>[] tf,
            ref Dictionary<string, double> itf, ref SparseMatrix matrix)
        {
            int rows = files.Length, columns = itf.Count(); // files count -> rows, words count (itf size) -> columns

            for (var row = 0; row < rows; row++)
            {
                var col = 0;
                var sum = 0d;

                foreach (var word in tf[row].Keys.ToList())
                {
                    var value = itf.ContainsKey(word) ? itf[word] * (tf[row][word] / columns) : 0d;
                    matrix[row, col] = value;
                    sum += value;
                    col++;
                }

                if (sum != 0)
                    for (col = 0; col < columns; col++)
                        if (matrix[row, col] != 0)
                            matrix[row, col] /= sum; // norlmalization

                Console.Write($"\r\tTF dictionary: {(row + 1).ToString().PadLeft(Program.Width, ' ')}/{rows}");
            }
        }


        /// <summary>
        ///     metoda matka obliczająca TF-ITF i wywołująca metody któe wykonują pomniejsze etapy
        /// </summary>
        public static SparseMatrix ComputeMatrix(ref string[] files)
        {
            var itf = new Dictionary<string, double>();
            var tf = new Dictionary<string, double>[Program.FilesCount];

            Console.Write($"{Program.Dir}\n\n1/7 cleaning, stemming, computing TF[] ...\n");

            var watch = Stopwatch.StartNew();
            Cleaner.ParseFiles(ref files, ref tf, ref itf);
            watch.Stop();

            Console.Write($"\nfinished in {Helper.GetHours(ref watch)}!\n\n2/7 modifing ITF ...\n");

            LogAndCleanItf(ref itf);
            BagOfWords = itf.Keys.ToArray();

            Console.Write("finished!\n\n3/7 converting TF[] to TF-ITF ...\n");

            watch.Restart();
            var matrix =
                new SparseMatrix(Program.FilesCount, BagOfWords.Length); //DoubleMatrix(files.Length, itf.Count());
            BuildNormalizedMatrix(ref files, ref tf, ref itf, ref matrix);
            watch.Stop();

            Console.Write($"\nfinished in {Helper.GetHours(ref watch)}!");

            return matrix;
        }
    }
}