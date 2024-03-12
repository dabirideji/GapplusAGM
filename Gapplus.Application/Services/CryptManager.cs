using BarcodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BarcodeGenerator.Service
{
    public class CryptManager
    {
        private const string EncryptionKey = "GAKKOJNNOOJYYVSLLKLHUIYYTTT";



        public static string Encrypt(string unencryptedText)
        {
            if (string.IsNullOrEmpty(unencryptedText))
                throw new GenericException("You cannot encrypt an empty text", "XRPT001");

            var hashmd5 = new MD5CryptoServiceProvider();
            var hashedKeyBytes = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(EncryptionKey));

            hashmd5.Clear();

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = hashedKeyBytes,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var unencryptedBytes = Encoding.UTF8.GetBytes(unencryptedText);
            var encryptedBytes = tdes.CreateEncryptor().TransformFinalBlock(unencryptedBytes, 0, unencryptedBytes.Length);

            tdes.Clear();

            return Convert.ToBase64String(encryptedBytes, 0, encryptedBytes.Length);
        }

        public static string Decrypt(string encryptedText)
        {
            //encryptedText = "cf+jReJi+nW0Aw1FaFXELWGq11gM1H9XrJBDNbf+q8NgnnGs/NMflix2rpEnCbhwgaf6l0iGSVv4e1TvWhq8DA==";
            if (string.IsNullOrEmpty(encryptedText))
                throw new GenericException("You cannot decrypt an empty text", "XRPT002");

            var md5Hasher = new MD5CryptoServiceProvider();
            var hashedKeyBytes = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(EncryptionKey));

            md5Hasher.Clear();

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = hashedKeyBytes,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var encryptedBytes = Convert.FromBase64String(encryptedText);
            var decryptedBytes = tdes.CreateDecryptor().TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            tdes.Clear();


            return Encoding.UTF8.GetString(decryptedBytes);
        }


        //public string base64urlDecode(string encoded)
        //{ 
        //    return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(encoded.Replace("_", "/").Replace("-", "+") + new string('=', (4 - encoded.Length % 4) % 4)));
        //}


    }

    public static class StringExtensions
    {
        public static string Encrypt(this string textToEncrypt)
        {
            return CryptManager.Encrypt(textToEncrypt);
        }

        public static string Decrypt(this string textToDecrypt)
        {
            return CryptManager.Decrypt(textToDecrypt);
        }
    }


    public class GenericException : SystemException
    {
        private string v;

        public GenericException(string message, string v) : base(message)
        {
            this.v = v;
            var log = new AppLog();
            log.ResponseCode = v;
            log.ResponseMessage = message;
            // using (UsersContext db = new UsersContext())
            // {
            //     db.AppLogs.Add(log);
            //     db.SaveChanges();

            // };
        }
    }

}