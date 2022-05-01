using System.Diagnostics;
using System.Net;

string WikiUrl = @"https://en.wikipedia.org/wiki/Special:Random";

Console.WriteLine("input ammount of random wiki articles to download:");

if (!int.TryParse(Console.ReadLine(), out int n)) return;


int width = n.ToString().Length;

string dir = $"{Directory.GetCurrentDirectory()}\\articles";

if (Directory.Exists(dir)) Directory.Delete(dir, true);

Directory.CreateDirectory(dir);

Console.WriteLine("starting the download");
Console.Write($"0/{n}");

Stopwatch watch = Stopwatch.StartNew();

for (int i = 1; i <= n; i++)
{
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(WikiUrl);
    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
    using (Stream stream = response.GetResponseStream())
    using (StreamReader reader = new StreamReader(stream))
    {
        File
            .CreateText($"{dir}\\article_{i.ToString().PadLeft(width, '0')}.html")
            .Write(reader.ReadToEnd());
        Console.Write($"\r{i}/{n}");
    }
}

watch.Stop();

Console.WriteLine($"\nfinished in {watch.ElapsedMilliseconds / 1000}s!\npress any key to quit ...");
Console.ReadKey();
Process.Start("explorer.exe", dir);
Environment.Exit(0);
