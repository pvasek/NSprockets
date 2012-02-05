using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NSprockets
{
    public static class Utils
    {

        public static string NormalizePath(this string name)
        {
            var result = name.Replace("\\", "/").ToLower().Trim();
            if (result.StartsWith("/"))
                result = result.Substring(1, result.Length - 1);
            if (result.EndsWith("/"))
                result = result.Substring(0, result.Length - 1);
            return result;
        }

        public static string GetHash(string content)
         {
             var hashAlg = MD5.Create();
             byte[] hashBytes = hashAlg.ComputeHash(Encoding.ASCII.GetBytes(content));
             return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
         }

        public static string GetOutputFileName(string inputFile, string outputPath, string hash)
        {
            string outputFileName = Path.GetFileNameWithoutExtension(inputFile);
            string outputFileExt = Path.GetExtension(inputFile);
            outputFileName = String.Format("{0}_{1}{2}", outputFileName, hash, outputFileExt);
            string outputFile = Path.Combine(outputPath, outputFileName);
            return outputFile;
        }
    }
}