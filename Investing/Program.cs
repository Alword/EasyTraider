using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Investing
{
    class Program
    {
        static bool update = true;
        static List<string> UI = new List<string>();
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            new Task(delegate { LoadData("https://ru.investing.com/currencies/btc-usd"); }).Start();
            new Task(delegate { LoadData("https://ru.investing.com/currencies/usd-rub"); }).Start();
            while (true)
            {
                System.Threading.Thread.Sleep(5000);
                if (update == true)
                {
                    Console.Clear();
                    foreach (string x in UI)
                    {
                        Console.WriteLine(x);
                    }
                    update = false;
                }
            }
        }

        static void LoadData(string page)
        {
            int myindex = 0;
            string lastdate = "";

            string Pair = page.Substring("/");
            while (Pair.Substring("/") != "")
            {
                Pair = Pair.Substring("/");
            }

            Regex time = new Regex(@">\d{2}:\d{2}:\d{2}");
            Regex pid = new Regex(@"\d{1,3}.\d{3},\d{1,3}");
            Regex percent = new Regex(@"[\-\+]\d{1,3},\d{1,2}%");


            lock (UI)
            {
                myindex = UI.Count;
                UI.Add("");
            }

            while (true)
            {
                var data = GetResponse(page);
                string document = data.Result;
                string stime = time.Match(document).Value;
                if (stime != lastdate)
                {
                    update = true;
                    lastdate = stime;
                    UI[myindex] = $"{lastdate} {Pair} {pid.Match(document).Value} {percent.Match(document).Value}";
                }
            }
        }

        private static async Task<string> GetResponse(string url)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");

            var response = await httpClient.GetAsync(new Uri(url));

            response.EnsureSuccessStatusCode();
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            //using (var decompressedStream = new System.IO.Compression.GZipStream(responseStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(responseStream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }

    public static class Extension
    {
        public static string Substring(this String input, string left, string right)
        {
            int indexL = input.IndexOf(left) + left.Length;
            int indexR = (input.Substring(indexL)).IndexOf(right);
            return ((indexL > -1) && indexR > -1) ? input.Substring(indexL, (input.Substring(indexL)).IndexOf(right)) : "";
        }

        public static string Substring(this String input, string left)
        {
            int indexL = input.IndexOf(left) + 1;
            int length = input.Length - indexL;
            return ((indexL > -1) && length < input.Length) ? input.Substring(indexL, length) : "";
        }
    }
}

//Regex regex = new Regex(@">BTC/USD \w*<");
//MatchCollection matches = regex.Matches(s);
//if (matches.Count > 0)
//{
//    foreach (Match match in matches)
//        Console.WriteLine(match.Value);
//}
//else
//{
//    Console.WriteLine("Совпадений не найдено");
//}
//int loading = 0;
//while (!data.IsCompleted)
//{
//    Console.Write($"Загрузка {loading++}");
//    System.Threading.Thread.Sleep(250);
//    Console.Clear();
//}
