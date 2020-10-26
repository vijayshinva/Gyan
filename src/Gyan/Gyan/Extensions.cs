using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Gyan
{
    static class Extensions
    {
        public static string ToSha256(this string text)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
            var stringBuilder = new StringBuilder();
            foreach (var b in bytes)
            {
                stringBuilder.Append(b.ToString("X2"));
            }
            return stringBuilder.ToString();
        }
    }
}
