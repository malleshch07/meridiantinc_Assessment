using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Net.Http.Headers;

namespace Meridianitinc_Assessment.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;

        public ApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            return await _client.GetAsync(endpoint);
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, object request)
        {
            return await _client.PostAsJsonAsync(endpoint, request);
        }
    }
}
