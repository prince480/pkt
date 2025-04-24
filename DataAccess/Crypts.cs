using ClosedXML.Excel;
using System;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PKT.DataAccess
{
    public class Crypts
    {
        private readonly IConfiguration _configuration;

        public Crypts(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                byte[] salt = new byte[32];

                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(salt);
                }

                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, salt);

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                byte[] salt = new byte[32];

                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(salt);
                }

                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, salt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string GetDecryptedValue(string uniqueIdentifier, string sessionId)
        {
            string url = "http://10.1.10.20:8888/ameyorestapi/customerManager/getDecryptedValueForField";
            string endPointUrl = $"{url}?uniqueIdentifier={uniqueIdentifier}";

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("sessionId", sessionId);

                HttpResponseMessage response = client.GetAsync(endPointUrl).Result;
                response.EnsureSuccessStatusCode();

                string result = response.Content.ReadAsStringAsync().Result;
                dynamic data = JObject.Parse(result);

                return data.decryptedValue;
            }
            catch (HttpRequestException e)
            {
                throw new Exception("HTTP request failed", e);
            }
        }


    }

}
