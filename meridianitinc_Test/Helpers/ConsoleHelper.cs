using System;
using System.Collections.Generic;
using System.Text;

namespace Meridianitinc_Assessment.Helpers
{
    public static class ConsoleHelper
    {
        public static async Task PrintResponse(HttpResponseMessage response)
        {
            Console.WriteLine($"Status: {response.StatusCode}");

            Console.WriteLine("\nHeaders:");

            foreach (var header in response.Headers)
            {
                Console.WriteLine($"{header.Key}: {string.Join(",", header.Value)}");
            }

            Console.WriteLine("\nBody:");

            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }
}
