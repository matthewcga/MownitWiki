using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using System.IO;
using WikiIndexer.FileHelpers;

namespace WikiIndexer
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Console.WriteLine("select folder with articles");
                string dir;
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    if (folderBrowserDialog.ShowDialog() != DialogResult.OK) throw new Exception("BAD OR NO FOLDER!");
                    dir = folderBrowserDialog.SelectedPath;
                }

                if (!Directory.Exists($"{dir}\\downloads"))
                    throw new Exception("NO DOWNLOADS IN FOLDER");

                Stopwatch watch = Stopwatch.StartNew();
                TfItfComputer.ComputeTFITF(ref dir);
                watch.Stop();

                Console.WriteLine($"\nfinished everything in {Helper.GetHours(ref watch)}!");
                Process.Start("explorer.exe", dir);
            }
            catch (Exception ex)
            { Console.WriteLine($"\n{ex.Message} at: {ex.TargetSite}\n\nstack trace:\n{ex.StackTrace}"); }
            finally
            {
                Console.Beep();
                Console.WriteLine($"\npress any key to quit ...");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}
