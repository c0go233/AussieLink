using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Services.HelperClasses
{
    public static class Encryptor
    {
        private static string Key = "ABC123DEF456GH78";

        private static byte[] GetByte(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }

        public static byte[] EncryptString(string data)
        {
            byte[] byteData = GetByte(data);
            SymmetricAlgorithm algo = SymmetricAlgorithm.Create();
            algo.Key = GetByte(Key);
            algo.GenerateIV();

            MemoryStream mStream = new MemoryStream();
            mStream.Write(algo.IV, 0, algo.IV.Length);

            CryptoStream cStream = new CryptoStream(mStream,
                algo.CreateEncryptor(), CryptoStreamMode.Write);
            cStream.Write(byteData, 0, byteData.Length);
            cStream.FlushFinalBlock();

            return mStream.ToArray();
        }

        public static string DecryptString(byte[] data)
        {
            SymmetricAlgorithm algo = SymmetricAlgorithm.Create();
            algo.Key = GetByte(Key);
            MemoryStream mStream = new MemoryStream();

            byte[] byteData = new byte[algo.IV.Length];
            Array.Copy(data, byteData, byteData.Length);
            algo.IV = byteData;

            int readFrom = 0;
            readFrom += algo.IV.Length;

            CryptoStream cStream = new CryptoStream(mStream,
                algo.CreateDecryptor(), CryptoStreamMode.Write);
            cStream.Write(data, readFrom, data.Length - readFrom);
            cStream.FlushFinalBlock();

            return Encoding.UTF8.GetString(mStream.ToArray());
        }

        public static string GetEncryptedString(string data)
        {
            return Convert.ToBase64String(EncryptString(data));
        }

        public static string GetDecryptedString(string data)
        {
            byte[] byteData = Convert.FromBase64String(data.Replace(" ", "+"));
            return DecryptString(byteData);
        }
    }
}
