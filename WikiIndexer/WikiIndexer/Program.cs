using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace WikiIndexer
{
    public static class Program
    {
        public static int FilesCount, Width;
        public static string Dir;

        [STAThread]
        private static void Main()
        {
            try
            {
                Console.WriteLine("select folder with articles");
                using (var folderBrowserDialog = new FolderBrowserDialog())
                {
                    if (folderBrowserDialog.ShowDialog() != DialogResult.OK) throw new Exception("BAD OR NO FOLDER!");
                    Dir = folderBrowserDialog.SelectedPath;
                    if (!Directory.Exists($"{Dir}\\downloads")) throw new Exception("NO DOWNLOADS IN FOLDER");
                }

                var files = Directory.GetFiles($"{Dir}\\downloads");
                FilesCount = files.Length;
                Width = FilesCount.ToString().Length;
                Helper.ValidateFolder(ref files);

                var watch = Stopwatch.StartNew();
                var matrix = Computer.ComputeMatrix(ref files);
                Exporter.ExportMatrix(ref matrix, ref files, false);
                Compressor.CompressMatrix(ref matrix);
                Exporter.ExportMatrix(ref matrix, ref files, true);
                watch.Stop();

                Console.WriteLine($"\n\nfinished everything in {Helper.GetHours(ref watch)}!");
                Process.Start("explorer.exe", Dir);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n{ex.Message} at: {ex.TargetSite}\n\nstack trace:\n{ex.StackTrace}");
            }
            finally
            {
                Console.WriteLine("\npress any key to quit ...");
                Console.ReadKey();
            }

            Environment.Exit(0);
        }
    }
}