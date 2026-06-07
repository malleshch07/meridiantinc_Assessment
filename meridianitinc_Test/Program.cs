using Meridianitinc_Assessment.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace meridiantinc_Assessment
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json");

            var config = builder.Build();

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    config["ApiKey"]);

            var apiService = new ApiService(httpClient);

            var result = await apiService.GetAsync(
    "https://ca-seassessment-api-dev.happywater-190f264d.northcentralus.azurecontainerapps.io/api/v1/health");

            Console.WriteLine(result);
        }
    }
}