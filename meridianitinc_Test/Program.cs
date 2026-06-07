using Meridianitinc_Assessment.Helpers;
using Meridianitinc_Assessment.Services;
using Microsoft.Extensions.Configuration;
using System.Buffers.Text;
using System.IO;
using System.Net.Http.Headers;
using System.Text;


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
            // ADD THE LOOP HERE
            for (int start = 0; start < 500; start += 100)
            {
                int end = start + 99;

                Console.WriteLine($"Downloading {start}-{end}");

                var response = await apiService.GetAsync(
                    $"{baseUrl}/api/v1/dataset?batch=true&range={start}-{end}");

                var content = await response.Content.ReadAsStringAsync();

                var fileName = $"batch-{start}-{end}.json";

                await File.WriteAllTextAsync(fileName, content);

                Console.WriteLine($"Saved {fileName}");
            }

            //        var response = await apiService.GetAsync(
            //$"{baseUrl}/api/v1/stats");


            //await ConsoleHelper.PrintResponse(response);


            //links used
            //        var response = await apiService.GetAsync(
            //$"{baseUrl}/api/v1/stats");
            //          var response = await apiService.GetAsync(
            //$"{baseUrl}/api/v1/dataset?batch=true&range=0-99");

            //          var submitResponse = await httpClient.PostAsync(
            //$"{baseUrl}/api/v1/submit",
            //new StringContent(
            //    """
            //      {
            //          "type":"test",
            //          "value":"test"
            //      }
            //      """,
            //    Encoding.UTF8,
            //    "application/json"));

            //var challengeResponse = await httpClient.PostAsync(
            //  $"{baseUrl}/api/v1/challenges",
            //  new StringContent("{}", Encoding.UTF8, "application/json"));

        }
    }
}