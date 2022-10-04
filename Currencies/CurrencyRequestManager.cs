using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Currencies
{
    internal class CurrencyRequestManager
    {
        private string usdUrl = "https://api.nbp.pl/api/exchangerates/rates/a/usd?format=json";
        private string eurUrl = "https://api.nbp.pl/api/exchangerates/rates/a/eur?format=json";
        private string chfUrl = "https://api.nbp.pl/api/exchangerates/rates/a/chf?format=json";

        public string GetUsdExchangeRate()
        {
            return GetExchangeRate(usdUrl);
        }

        public string GetEurExchangeRate()
        {
            return GetExchangeRate(eurUrl);
        }

        public string GetChfExchangeRate()
        {
            return GetExchangeRate(chfUrl);
        }

        public string GetGoldRate()
        {
            string url = "https://api.nbp.pl/api/cenyzlota?format=json";

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var responseMessage = httpClient.SendAsync(request).Result;
                var responseContent = responseMessage.Content.ReadAsStringAsync().Result;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                GoldInfo[] goldInfos = javaScriptSerializer.Deserialize<GoldInfo[]>(responseContent);

                return goldInfos.First().cena;
            }
        }

        private string GetExchangeRate(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var responseMessage = httpClient.SendAsync(request).Result;
                var responseContent = responseMessage.Content.ReadAsStringAsync().Result;

                var currencyInfo = JsonConvert.DeserializeObject<CurrencyInfo>(responseContent);

                return currencyInfo.Rates.First().Mid;
            }
        }
    }

    class CurrencyInfo
    {
        public string Table { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public List<CurrencyRateInfo> Rates { get; set; }
    }

    class CurrencyRateInfo
    {
        public string No { get; set; }
        public string EffectiveDate { get; set; }
        public string Mid { get; set; }
    }

    class GoldInfo
    {
        public string data { get; set; }
        public string cena { get; set; }
    }
}
