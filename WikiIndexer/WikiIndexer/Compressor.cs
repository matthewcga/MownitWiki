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
            //DoubleSVDecompServer server = new DoubleSVDecompServer() { ComputeFull = true, InPlace = true, Tolerance = Helper.SVDTolerance };
            //DoubleComplexGSVDecomp svd = server.GetDecomp();//server.GetDecomp(new DoubleCsrSparseMatrix(matrix, Computer.BagOfWords.Length).ToDenseMatrix());
            //return (svd.LeftVectors, svd.RightVectors, svd.SingularValues);

            var svd = matrix.Svd();
            return (svd.U, svd.VT, svd.S);
        }

        private static SparseMatrix RebuildMatrix(ref Matrix<double> u, ref Matrix<double> v, ref Vector<double> s,
            string test)
        {
            var matrix = new SparseMatrix(Program.FilesCount, Computer.BagOfWords.Length);
            var i = 0;
            double lim = s[0] * (1d - Helper.SvdCompressionRate), value;


            var path = Path.GetTempFileName();
            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine("\n# # # # # # # # # #\n\tcompression iteration: 0\n# # # # # # # # # #\n");
                sw.WriteLine(test);

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

                    sw.WriteLine($"\n\n# # # # # # # # # #\n\tcompression iteration: {i}\n# # # # # # # # # #\n");
                    sw.WriteLine(matrix.ToString());
                }
            }

            Process.Start("notepad.exe", path);

            return matrix;
        }

        public static void CompressMatrix(ref SparseMatrix matrix)
        {
            Console.Write("\n\n4/7 decomposing matrix ...\n");

            var test = matrix.ToString();

            var watch = Stopwatch.StartNew();
            var (u, v, s) = SvdMatrix(ref matrix);
            watch.Stop();

            Console.Write($"finished in {Helper.GetHours(ref watch)}!\n\n5/7 rebuilding matrix ...\n");

            watch.Restart();
            matrix = RebuildMatrix(ref u, ref v, ref s, test);
            watch.Stop();

            Console.Write($"finished in {Helper.GetHours(ref watch)}!");
        }
    }
}