using System;
using System.Diagnostics;
using System.IO;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace WikiIndexer
{
    public static class Compressor
    {
        private static (Matrix<double>, Matrix<double>, Vector<double>) SvdMatrix(ref SparseMatrix matrix)
        {
            var svd = matrix.Svd();
            return (svd.U, svd.VT, svd.S);
        }

        private static SparseMatrix RebuildMatrix(ref Matrix<double> u, ref Matrix<double> v, ref Vector<double> s)
        {
            var matrix = new SparseMatrix(Program.FilesCount, Computer.BagOfWords.Length);
            var i = 0;
            double lim = s[0] * (1d - Helper.SvdCompressionRate), value;

            while (i < s.Count && s[i] > lim)
            {
                for (var uRow = 0; uRow < Program.FilesCount; uRow++)
                    for (var vCol = 0; vCol < Computer.BagOfWords.Length; vCol++)
                    {
                        value = u[uRow, i] * v[i, vCol] * s[i];
                        if (value != 0d)
                            matrix[uRow, vCol] += value;
                    }
                i++;
            }

            return matrix;
        }

        public static void CompressMatrix(ref SparseMatrix matrix)
        {
            Console.Write("\n\ndecomposing matrix ...\n");

            var watch = Stopwatch.StartNew();
            var (u, v, s) = SvdMatrix(ref matrix);
            watch.Stop();

            Console.Write($"finished in {Helper.GetHours(ref watch)}!\n\nrebuilding matrix ...\n");

            watch.Restart();
            matrix = RebuildMatrix(ref u, ref v, ref s);
            watch.Stop();

            Console.Write($"finished in {Helper.GetHours(ref watch)}!");
        }
    }
}