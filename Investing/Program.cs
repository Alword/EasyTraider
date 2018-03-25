using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Investing
{
    public class BuisnessData
    {
        public string Pair { get; set; }
        public double Pid { get; set; }
        public double Percent { get; set; }
        public DateTime AccessDate { get; set; }

        public int TotalSeconds
        {
            get
            {
                return (int)Math.Truncate((DateTime.Now - AccessDate).TotalSeconds);
            }
        }
    }

    class Program
    {
        static List<BuisnessData> UI = new List<BuisnessData>();
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            new System.Threading.Thread((() => { LoadData("https://ru.investing.com/currencies/usd-rub"); })).Start();
            new System.Threading.Thread((() => { LoadData("https://ru.investing.com/currencies/btc-usd"); })).Start();
            CBuffer console = new CBuffer(Console.WindowHeight, Console.WindowWidth);
            Console.CursorVisible = false;

            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                console.Revert();
                foreach (BuisnessData x in UI)
                {
                    console.WriteLine($"{x.Pair} | {x.Pid} | {x.Percent} | ({x.TotalSeconds} seconds ago)");
                }
                console.WriteLine("[Press SPACE to exit]");
                console.Revert();
                console.Refresh();
            }
        }

        static void LoadData(string page)
        {
            var updateData = new BuisnessData
            {
                Pair = page.Substring(page.LastIndexOf("/") + 1)
            };

            Regex time = new Regex(@">\d{2}:\d{2}:\d{2}");
            Regex pid = new Regex(@"\d{1,3}.\d{3},\d{1,3}");
            Regex percent = new Regex(@"[\-\+]\d{1,3},\d{1,2}%");

            lock (UI)
            {
                UI.Add(updateData);
            }

            while (true)
            {
                var data = GetResponse(page);
                DateTime AccessTime = DateTime.Now;
                string document = data.Result;


                string documentData = time.Match(document).Value;
                if (documentData != "")
                {
                    AccessTime = Convert.ToDateTime(time.Match(document).Value.Substring(1));
                }
                else
                {
                    updateData.AccessDate = AccessTime;
                    if (!updateData.Pair.Contains("[Closed]"))
                    {
                        updateData.Pair = $"[Closed] {updateData.Pair}";
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(new TimeSpan(0, 2, 0));
                    }
                }
                if (AccessTime != updateData.AccessDate)
                {
                    updateData.AccessDate = AccessTime;
                    updateData.Pid = Convert.ToDouble(pid.Match(document).Value.Replace(".", ""));
                    updateData.Percent = Convert.ToDouble(percent.Match(document).Value.Replace("%", ""));
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
