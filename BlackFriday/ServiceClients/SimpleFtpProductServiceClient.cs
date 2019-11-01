using BlackFriday.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BlackFriday.ServiceClients
{
    public class SimpleFtpProductServiceClient
    {
        private readonly HttpClient _client;
        private ILogger<SimpleCreditCartServiceClient> _logger;

        private static ArrayList baseUrls = new ArrayList();
        

        private static int currentUsedUrl = 0;

        public SimpleFtpProductServiceClient(HttpClient client, ILogger<SimpleCreditCartServiceClient> logger)
        {
            _client = client;
            _logger = logger;
            initBaseUrls();

        }

        private void initBaseUrls()
        {
            baseUrls.Add("https://localhost:44355/api/ProductFtp");
        }
    

        private bool BaseUrlsAvailable()
        {
            return (baseUrls != null && baseUrls.Count > 0);
        }

        private string GetRoundRobinBaseUrl()
        {
            //Stay in range! 0..(baseUrls.leng-1)
            currentUsedUrl = (currentUsedUrl < 0 || currentUsedUrl >= (baseUrls.Count-1)) ? 0 : currentUsedUrl;

            string baseUrl = baseUrls[currentUsedUrl].ToString();
            _logger.LogError("hoedlale16: [" + currentUsedUrl + "] Use Service-URL <" + baseUrl + ">");
            //Increase counter to next Urls
            currentUsedUrl++;

            return baseUrl;
        }

        public async Task<List<string>> GetProductsFromFtp()
        {
            if(! BaseUrlsAvailable() )
            {
                _logger.LogError("No futher services available - This service is totaly broken");
            } else
            {
                try
                {
                    _client.DefaultRequestHeaders.Accept.Clear();                    

                    string baseUrl = GetRoundRobinBaseUrl();
                    var response = await _client.GetAsync(baseUrl + "/api/ProductFtp");

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsAsync<List<string>>();
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError("An error occurred connecting to SimpleCreditCartServiceClient");

                    //Call next Service. This is probably down or has another issue (Some crazy shit is going on with this service...)
                    //Endlos-Loop prevention is done by CircuitBreakerAsync
                    await GetProductsFromFtp();
                }
            }

            //Fallback return empty list
            return new List<string>();
        }
    }
}
