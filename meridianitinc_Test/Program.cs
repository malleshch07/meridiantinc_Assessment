using Meridianitinc_Assessment.Helpers;
using Meridianitinc_Assessment.Services;
using Microsoft.Extensions.Configuration;
using System.Buffers.Text;
using System.Net.Http.Headers;

namespace meridiantinc_Assessment
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
     .AddJsonFile("appsettings.json")
     .Build();

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    config["ApiKey"]);

            var apiService = new ApiService(httpClient);
            var baseUrl = config["BaseUrl"];
            Console.WriteLine($"BaseUrl: {baseUrl}");
            Console.WriteLine("Assessment client ready.");

            var response = await apiService.GetAsync($"{baseUrl}/");

            await ConsoleHelper.PrintResponse(response);
        }
    }
}