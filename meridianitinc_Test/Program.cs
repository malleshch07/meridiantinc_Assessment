using Microsoft.Extensions.Configuration;
using Meridianitinc_Assessment.Helpers;
using Meridianitinc_Assessment.Models;
using Meridianitinc_Assessment.Services;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;

namespace Meridianitinc_Assessment
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

            // =====================================
            // STEP 1 - GET ASSESSMENT STATS
            // =====================================

            Console.WriteLine("\n===== STATS =====");

            var statsResponse =
                await apiService.GetAsync(
                    $"{baseUrl}/api/v1/stats");

            Console.WriteLine(
                await statsResponse.Content.ReadAsStringAsync());

            // =====================================
            // STEP 2 - DOWNLOAD DATASET
            // =====================================

            Console.WriteLine("\n===== DOWNLOAD DATASET =====");

            var dataFolder =
                @"C:\Users\malle\source\repos\meridianitinc_Test\meridianitinc_Test\Data";

            Directory.CreateDirectory(dataFolder);

            for (int start = 0; start < 500; start += 100)
            {
                int end = start + 99;

                Console.WriteLine(
                    $"Downloading {start}-{end}");

                var response =
                    await apiService.GetAsync(
                        $"{baseUrl}/api/v1/dataset?batch=true&range={start}-{end}");

                var content =
                    await response.Content.ReadAsStringAsync();

                var fileName =
                    Path.Combine(
                        dataFolder,
                        $"batch-{start}-{end}.json");

                await File.WriteAllTextAsync(
                    fileName,
                    content);

                Console.WriteLine(
                    $"Saved {fileName}");
            }

            // =====================================
            // STEP 3 - READ ALL FILES
            // =====================================

            Console.WriteLine("\n===== READ DATASET =====");

            var allRecords = new List<string>();

            var files =
                Directory.GetFiles(
                    dataFolder,
                    "*.json");

            foreach (var file in files.OrderBy(f => f))
            {
                Console.WriteLine(
                    $"Reading {Path.GetFileName(file)}");

                var json =
                    await File.ReadAllTextAsync(file);

                var dataset =
                    JsonSerializer.Deserialize<DatasetResponse>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (dataset != null)
                {
                    Console.WriteLine(
                        $"Found {dataset.Data.Count} records");

                    allRecords.AddRange(
                        dataset.Data);
                }
            }

            Console.WriteLine(
                $"Total Records: {allRecords.Count}");

            // =====================================
            // STEP 4 - VALIDATE DATASET
            // =====================================

            Console.WriteLine("\n===== VALIDATION =====");

            Console.WriteLine(
                $"Duplicates: {allRecords.Count - allRecords.Distinct().Count()}");

            Console.WriteLine(
                $"First Length: {allRecords.First().Length}");

            Console.WriteLine(
                $"Last Length: {allRecords.Last().Length}");

            // =====================================
            // STEP 5 - CONTENT HASH TEST
            // =====================================

            Console.WriteLine("\n===== CONTENT HASH =====");

            var combined =
                string.Join("", allRecords);

            var contentHash =
                HashHelper.ComputeSha256(combined);

            Console.WriteLine(contentHash);

            // =====================================
            // STEP 6 - RAW FILE HASH TEST
            // =====================================

            Console.WriteLine("\n===== RAW FILE HASH =====");

            using var ms = new MemoryStream();

            foreach (var file in files.OrderBy(f => f))
            {
                var bytes =
                    await File.ReadAllBytesAsync(file);

                ms.Write(bytes);
            }

            var rawFileHash =
                Convert.ToHexString(
                    SHA256.HashData(ms.ToArray()))
                .ToLower();

            Console.WriteLine(rawFileHash);

            // =====================================
            // STEP 7 - PAGED DATASET TEST
            // =====================================

            Console.WriteLine("\n===== PAGED DATASET =====");

            var pagedRecords =
                new List<string>();

            for (int page = 1; page <= 20; page++)
            {
                bool success = false;

                while (!success)
                {
                    var response =
                        await apiService.GetAsync(
                            $"{baseUrl}/api/v1/dataset?page={page}");

                    Console.WriteLine(
                        $"Page {page} => {response.StatusCode}");

                    if (response.IsSuccessStatusCode)
                    {
                        var json =
                            await response.Content.ReadAsStringAsync();

                        var dataset =
                            JsonSerializer.Deserialize<DatasetResponse>(
                                json,
                                new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                });

                        pagedRecords.AddRange(
                            dataset.Data);

                        success = true;
                    }
                    else
                    {
                        Console.WriteLine(
                            await response.Content.ReadAsStringAsync());

                        await Task.Delay(2000);
                    }
                }

                await Task.Delay(1200);
            }

            Console.WriteLine(
                $"Records: {pagedRecords.Count}");

            var canonicalJson =
                JsonSerializer.Serialize(
                    new
                    {
                        data = pagedRecords
                    });

            var pagedHash =
                HashHelper.ComputeSha256(
                    canonicalJson);

            Console.WriteLine(
                $"Canonical Hash: {pagedHash}");

            // =====================================
            // STEP 8 - CHALLENGE DISCOVERY
            // =====================================

            Console.WriteLine("\n===== CHALLENGES =====");

            var challengeResponse =
                await apiService.GetAsync(
                    $"{baseUrl}/api/v1/challenges");

            Console.WriteLine(
                await challengeResponse.Content.ReadAsStringAsync());

            Console.WriteLine("\nAssessment Complete");

            #region tested end points
            //        var responseaa = await apiService.GetAsync(
            //$"{baseUrl}/api/v1/stats");


            //        await ConsoleHelper.PrintResponse(responseaa);


            //links used



            //var payload = new
            //{
            //    type = "content_hash",
            //    value = "48b0077aeec0be19835c56cf3315085606e94a1a6229760efe90b5324c1fc10a"
            //};

            //var response = await apiService.PostAsync(
            //    $"{baseUrl}/api/v1/submit",
            //    payload);

            //await ConsoleHelper.PrintResponse(response);


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
            //       var request = new HttpRequestMessage(
            //HttpMethod.Options,
            //$"{baseUrl}/api/v1/challenges");
            #endregion
        }
    }
}

