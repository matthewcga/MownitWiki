using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace WikiDownloader
{
    internal class Program
    {
        private static readonly string WikiUrl = @"https://en.wikipedia.org/wiki/Special:Random";

        [STAThread]
        static void Main()
        {
            try
            {
                Console.WriteLine("input ammount of random wiki articles to download and select the download folder:");

                if (!int.TryParse(Console.ReadLine(), out int n)) throw new InvalidCastException("NOT A NUMBER!");
                int width = n.ToString().Length;

                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK) throw new Exception("BAD OR NO FOLDER!");
                string dir = folderBrowserDialog.SelectedPath;

                if (!Directory.Exists($"{dir}\\downloads"))
                    Directory.CreateDirectory($"{dir}\\downloads");

                Console.WriteLine("\nstarting the download\n");

                Stopwatch watch = Stopwatch.StartNew(), downloadWatch = new Stopwatch();
                using (StreamWriter sw = File.CreateText($"{dir}\\links.json"))
                {
                    sw.Write("[\n");
                    for (int i = 1; i <= n; i++)
                        DownloadAction(downloadWatch, dir, i, n, width, sw);
                    sw.Write("]");
                    sw.Close();
                }
                watch.Stop();

                Console.WriteLine($"\nfinished downloading in {GetHours(ref watch)}!");
                Process.Start("explorer.exe", dir);
            }
            catch (Exception ex)
            { Console.WriteLine($"\n{ex.Message} at: {ex.TargetSite}\nstack trace:\n{ex.StackTrace}"); }
            finally
            {
                Console.Beep();
                Console.WriteLine($"\npress any key to quit ...");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        private static void DownloadAction(Stopwatch watch, string dir, int i, int n, int width, StreamWriter jsonSw)
        {
            string html = $"{dir}\\downloads\\article_{i.ToString().PadLeft(width, '0')}.html";
            watch.Restart();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(WikiUrl);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter sw = File.CreateText(html))
            {
                jsonSw.WriteLine($"{JsonSerializer.Serialize(new KeyValuePair<string, string>(Path.GetFileName(html), response.ResponseUri.ToString()))},");
                sw.Write(reader.ReadToEnd());
                watch.Stop();
                Console.WriteLine($"\t{i.ToString().PadLeft(width, ' ')}/{n}, downloaded in {watch.Elapsed.TotalMilliseconds} ms: '{response.ResponseUri}'");
                sw.Close();
            }
        }

        private static string GetHours(ref Stopwatch watch) =>
            $"{watch.Elapsed.Hours} h {watch.Elapsed.Minutes - (watch.Elapsed.Hours * 60)} min {watch.Elapsed.Seconds - (watch.Elapsed.Minutes * 60)} sec";
    }
}
