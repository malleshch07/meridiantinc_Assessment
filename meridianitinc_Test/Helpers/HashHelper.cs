using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
namespace Meridianitinc_Assessment.Helpers
{
    public static class HashHelper
    {
        public static string ComputeSha256(string input)
        {
            using var sha = SHA256.Create();

            var bytes = Encoding.UTF8.GetBytes(input);

            var hash = sha.ComputeHash(bytes);

            return Convert.ToHexString(hash).ToLower();
        }
    }
}
