using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using WikiIndexer.FileHelpers;

namespace WikiIndexer
{
    public static class Program
    {
        public static string Dir;

        [STAThread]
        static void Main()
        {
            try
            {
                Console.WriteLine("select folder with articles");
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    if (folderBrowserDialog.ShowDialog() != DialogResult.OK) throw new Exception("BAD OR NO FOLDER!");
                    Dir = folderBrowserDialog.SelectedPath;
                    if (!Directory.Exists($"{Dir}\\downloads")) throw new Exception("NO DOWNLOADS IN FOLDER");
                }

                Stopwatch watch = Stopwatch.StartNew();
                TfItfComputer.ComputeTFITF();
                watch.Stop();

                Console.WriteLine($"\nfinished everything in {Helper.GetHours(ref watch)}!");
                Process.Start("explorer.exe", Dir);
            }
            catch (Exception ex) { Console.WriteLine($"\n{ex.Message} at: {ex.TargetSite}\n\nstack trace:\n{ex.StackTrace}"); }
            finally
            {
                Console.WriteLine($"press any key to quit ...");
                Console.ReadKey();
            }
            Environment.Exit(0);
        }
    }
}
