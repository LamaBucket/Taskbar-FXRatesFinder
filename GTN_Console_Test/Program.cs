using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;

namespace GTN_Console_Test
{
    internal class Program
    {
        private const string apiKey = "b38469fba4f73b54782a";
        private const string url = "https://free.currconv.com/api/v7/convert";
        private const string baseCurrencyName = "RUB";
        private const string targetCurrencryName = "USD";
        private static string FXRateName = $"{targetCurrencryName}_{baseCurrencyName}";


        static void Main(string[] args)
        {
            while (true)
            {
                PerformRequestAndWriteData();
                Thread.Sleep(60000);
            }
        }

        private static void PerformRequestAndWriteData()
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString.Add("apiKey", apiKey);
            queryString.Add("q", FXRateName);
            queryString.Add("compact", "ultra");

            UriBuilder uri = new UriBuilder(url);

            uri.Query = queryString.ToString();

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(uri.Uri);

                JObject jo = JObject.Load(new JsonTextReader(new StringReader(json)));

                decimal value = Convert.ToDecimal(jo.GetValue(FXRateName));

                Console.WriteLine(value);
            }


            //request.RequestUri.Query

        }
    }
}
