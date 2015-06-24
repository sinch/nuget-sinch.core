using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sinch.Core
{
    public class Client : IDisposable
    {
        private string _applicationKey;
        private string _applicationSecret;
        private Sinch.Core.Security security;

        public Client()
        {
            throw new Exception("Client must be initialized with kay and secret");
        }

        public Client(string applicationKey, string applicationSecret)
        {
            _applicationKey = applicationKey;
            _applicationSecret = applicationSecret;
            security = new Security(applicationKey, applicationSecret);
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var uriBuilder = new UriBuilder(url);
            using (var httpClient = new HttpClient())
            {
                var timestamp = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
                httpClient.DefaultRequestHeaders.Add("x-timestamp", timestamp);
                httpClient.DefaultRequestHeaders.Add("Authorization",
                    "application " + security.SignGetRequest("GET", uriBuilder.Path, timestamp));
                return await httpClient.GetAsync(url);
            }
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T value)
        {
            var uriBuilder = new UriBuilder(url);
            using (var httpClient = new HttpClient())
            {
                var timestamp = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
                httpClient.DefaultRequestHeaders.Add("x-timestamp", timestamp);
                httpClient.DefaultRequestHeaders.Add("Authorization",
                    "application " +
                    security.SignRequest("POST", JsonConvert.SerializeObject(value), uriBuilder.Path, timestamp));
                return await httpClient.PostAsJsonAsync(url, value);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
        }
    }
}
