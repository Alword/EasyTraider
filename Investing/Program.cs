using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Investing
{
    class Program
    {
        static string lastdate = "";
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                LoadData();
                System.Threading.Thread.Sleep(5000);
            }
        }

        static void LoadData()
        {
            string page = "https://ru.investing.com/currencies/btc-usd";
            var data = GetResponse(page);

            string date = data.Result.Substring("time\">", "</span>");
            string s = data.Result.Substring("last_last", "<");
            string resume = data.Result.Substring("Резюме</td>", "</td>");

            string result = resume.Substring(">");
            if (result.IndexOf("прод") > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (result.IndexOf("пок") > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (date != lastdate)
            {
                Console.WriteLine($"[{date}] {s.Substring(">")} {result}");
                lastdate = date;
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
