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
        private static int N, Width;
        private static string Dir;

        [STAThread]
        static void Main()
        {
            try
            {
                Console.WriteLine("input ammount of wiki articles");
                if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0) throw new InvalidCastException("NOT A NUMBER OR 0!");
                N = n; Width = N.ToString().Length;

                Console.WriteLine("select folder for download");
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK) throw new Exception("BAD OR NO FOLDER!");
                Dir = folderBrowserDialog.SelectedPath;
                if (!Directory.Exists($"{Dir}\\downloads")) Directory.CreateDirectory($"{Dir}\\downloads");

                Console.WriteLine($"{Dir}\n\nstarting the download ...");

                Stopwatch watch = Stopwatch.StartNew();
                DownloadAction();
                watch.Stop();

                Console.WriteLine($"\n\n\nfinished downloading in {GetHours(ref watch)}!");
                Process.Start("explorer.exe", Dir);
            }
            catch (Exception ex) { Console.WriteLine($"\n{ex.Message} at: {ex.TargetSite}\nstack trace:\n{ex.StackTrace}"); }
            finally
            {
                Console.WriteLine($"press any key to quit ...");
                Console.ReadKey();
            }
            Environment.Exit(0);
        }

        /// <summary>
        /// Pobiera zadą ilość plików i tworzy .json z mapą [nazwa pliku : link do oryginału]
        /// </summary>
        private static void DownloadAction()
        {
            using (StreamWriter jsonSw = File.CreateText($"{Dir}\\links.json"))
            {
                jsonSw.Write("[\n");
                for (int i = 1; i <= N; i++)
                    DownloadFile(i, jsonSw);
                jsonSw.Write("]");
                jsonSw.Close();
            }
        }

        /// <summary>
        /// Pobiera pojedyńczy plik z wiki
        /// </summary>
        /// <param name="i">numer pobieranego pliku</param>
        /// <param name="jsonSw">link do .json z mapą</param>
        private static void DownloadFile(int i, StreamWriter jsonSw)
        {
            Stopwatch watch = Stopwatch.StartNew();
            string html = $"{Dir}\\downloads\\article_{i.ToString().PadLeft(Width, '0')}.html";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(WikiUrl);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter sw = File.CreateText(html))
            {
                jsonSw.WriteLine($"{JsonSerializer.Serialize(new KeyValuePair<string, string>(Path.GetFileName(html), response.ResponseUri.ToString()))},");
                sw.Write(reader.ReadToEnd());
                watch.Stop();
                Console.Write($"\r\tfile: {i.ToString().PadLeft(Width, ' ')}/{N}, time: {watch.Elapsed.TotalMilliseconds} ms");
                sw.Close();
            }
        }

        /// <summary>
        /// Buduje napis z stopera
        /// </summary>
        /// <param name="watch">stoper</param>
        /// <returns>utworzony napis (h, min, sec)</returns>
        private static string GetHours(ref Stopwatch watch) =>
            $"{watch.Elapsed.Hours} h {watch.Elapsed.Minutes - (watch.Elapsed.Hours * 60)} min {watch.Elapsed.Seconds - (watch.Elapsed.Minutes * 60)} sec";
    }
}
