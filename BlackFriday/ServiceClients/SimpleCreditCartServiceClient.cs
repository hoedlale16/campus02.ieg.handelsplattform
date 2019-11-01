using BlackFriday.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BlackFriday.ServiceClients
{
    public class SimpleCreditCartServiceClient
    {
        private readonly HttpClient _client;

        private readonly string[] urls =
        {
            "bla","bla"
        };

        public SimpleCreditCartServiceClient()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"http://iegGr8easycreditcardservice.azurewebsites.net/")
            };
        }

        public async Task<bool> PostCreditcartTransaction(CreditcardTransaction creditCardTransaction)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(creditCardTransaction), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.PostAsync(new Uri($"/api/CreditcardTransactions"), httpContent);
            return (response.IsSuccessStatusCode) ?  true :  false;
        }


        public async Task<List<string>> GetPaymentMethods()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.GetAsync(new Uri($"/api/AcceptedCreditCards"));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<string>>();
            }

            return null;
        }
    }
}
