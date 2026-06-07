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
            Directory.CreateDirectory(
    @"C:\Users\malle\source\repos\meridianitinc_Test\meridianitinc_Test\Data");
           
            for (int start = 0; start < 500; start += 100)
            {
                int end = start + 99;

                Console.WriteLine($"Downloading {start}-{end}");

                var response = await apiService.GetAsync(
                    $"{baseUrl}/api/v1/dataset?batch=true&range={start}-{end}");

                var content = await response.Content.ReadAsStringAsync();

                var fileName =
      $@"C:\Users\malle\source\repos\meridianitinc_Test\meridianitinc_Test\Data\batch-{start}-{end}.json";

                await File.WriteAllTextAsync(fileName, content);

                Console.WriteLine($"Saved {fileName}");
                Console.WriteLine(Path.GetFullPath(fileName));
            }

            Console.WriteLine("\nSearching for key endpoint...");

            string[] endpoints =
            {
    "/",
    "/api/v1",
    "/swagger",
    "/swagger/index.html",
    "/openapi.json",
    "/robots.txt"
};

            foreach (var ep in endpoints)
            {
                try
                {
                    Console.WriteLine($"\nTesting {ep}");

                    var response = await httpClient.GetAsync(ep);

                    Console.WriteLine($"Status: {response.StatusCode}");

                    var body = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(body);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
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