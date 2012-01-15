using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NSprockets
{
    public static class Utils
    {
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