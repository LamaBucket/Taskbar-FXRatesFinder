using GTN_WPF.Model.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;

namespace GTN_WPF.Model.Implementations
{
    internal class DataProviderWeb : IDataProvider
    {
        public DataProviderWeb(string apiKey)
        {
            this.apiKey = apiKey;
        }

        private string apiKey;

        private static readonly string url1 = "https://api.freecurrencyapi.com/v1/latest";
        private static readonly string url2 = "https://api.freecurrencyapi.com/v1/currencies";

        public FXRate GetFXRate(string localCurrencyName, string targetCurrencyName)
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString.Add("apikey", apiKey);
            queryString.Add("currencies", targetCurrencyName);
            queryString.Add("base_currency", localCurrencyName);

            UriBuilder uri = new UriBuilder(url1);

            uri.Query = queryString.ToString();

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(uri.Uri);

                if (!string.IsNullOrEmpty(json))
                {
                    decimal? value = JObject.Parse(json)?["data"]?[targetCurrencyName]?.Value<decimal>();

                    return new FXRate(value.HasValue ? value.Value : -999, targetCurrencyName);
                }
            }

            return FXRate.Unknown;
        }

        public IList<FXRate> SearchRates(string searchFilter)
        {
            IList<FXRate> rates = new List<FXRate>();

            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString.Add("apikey", apiKey);

            UriBuilder uri = new UriBuilder(url2);

            uri.Query = queryString.ToString();

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(uri.Uri);

                if (!string.IsNullOrEmpty(json))
                {
                    dynamic obj = JsonConvert.DeserializeObject(json) ?? throw new InvalidCastException();

                    string obj2 = Convert.ToString(obj.data);

                    Dictionary<String, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(obj2) ?? throw new InvalidDataException();

                    foreach (string key in dict.Keys)
                    {
                        if (string.IsNullOrEmpty(searchFilter) || key.Contains(searchFilter))
                        {
                            rates.Add(new FXRate(-999, key));
                        }

                    }
                }
            }

            return rates;
        }
    }
}
