using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Windows.Forms;

namespace WikiDownloader
{
    internal class Program
    {
        private const string WikiUrl = @"https://en.wikipedia.org/wiki/Special:Random";
        private static int N, Width;
        private static string Dir;

        [STAThread]
        private static void Main()
        {
            try
            {
                Console.WriteLine("input amount of wiki articles");
                if (!int.TryParse(Console.ReadLine(), out var n) || n <= 0)
                    throw new InvalidCastException("NOT A NUMBER OR 0!");
                N = n;
                Width = N.ToString().Length;

                Console.WriteLine("select folder for download");
                var folderBrowserDialog = new FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK) throw new Exception("BAD OR NO FOLDER!");
                Dir = folderBrowserDialog.SelectedPath;
                if (!Directory.Exists($"{Dir}\\downloads")) Directory.CreateDirectory($"{Dir}\\downloads");

                Console.WriteLine($"{Dir}\n\nstarting the download ...");

                var watch = Stopwatch.StartNew();
                DownloadAction();
                watch.Stop();

                Console.WriteLine($"\n\n\nfinished downloading in {GetHours(ref watch)}!");
                Process.Start("explorer.exe", Dir);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n{ex.Message} at: {ex.TargetSite}\nstack trace:\n{ex.StackTrace}");
            }
            finally
            {
                Console.WriteLine("press any key to quit ...");
                Console.ReadKey();
            }

            Environment.Exit(0);
        }


        private static void DownloadAction()
        {
            using (var jsonSw = File.CreateText($"{Dir}\\links.json"))
            {
                jsonSw.Write("[\n");
                for (var i = 1; i <= N; i++)
                    DownloadFile(i, jsonSw);
                jsonSw.Write("]");
                jsonSw.Close();
            }
        }


        private static void DownloadFile(int i, StreamWriter jsonSw)
        {
            var watch = Stopwatch.StartNew();
            var html = $"{Dir}\\downloads\\article_{i.ToString().PadLeft(Width, '0')}.html";
            var request = (HttpWebRequest)WebRequest.Create(WikiUrl);
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            using (var sw = File.CreateText(html))
            {
                jsonSw.WriteLine(
                    $"{JsonSerializer.Serialize(new KeyValuePair<string, string>(Path.GetFileName(html), response.ResponseUri.ToString()))},");
                sw.Write(reader.ReadToEnd());
                watch.Stop();
                Console.Write(
                    $"\r\tfile: {i.ToString().PadLeft(Width, ' ')}/{N}, time: {GetMs(ref watch)} ms");
                sw.Close();
            }
        }

        public static string GetMs(ref Stopwatch watch)
        {
            return string.Format("{0:D2} sec", watch.Elapsed.Milliseconds);
        }


        public static string GetHours(ref Stopwatch watch)
        {
            return string.Format("{0:D2} h {1:D2} m {2:D2} sec",
                watch.Elapsed.Hours,
                watch.Elapsed.Minutes,
                watch.Elapsed.Seconds);
        }
    }
}