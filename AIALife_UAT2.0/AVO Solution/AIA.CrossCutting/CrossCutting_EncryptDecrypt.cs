using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;


namespace AIA.CrossCutting
{
    public class CrossCutting_EncryptDecrypt
    {
        public static string Encrypt(string textValue, string encryptKey = null)
        {
            if (string.IsNullOrEmpty(encryptKey))
            {
                encryptKey = ConfigurationSettings.AppSettings["EncryptDecryptKey"];
            }
            if (textValue != null)
            {

                string passPhrase = "Pas5pr@se";
                int passwordIterations = 2;
                string hashAlgorithm = "SHA1";
                string initVector = "@1B2c3D4e5F6g7H8";
                int keySize = 128;

                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(encryptKey);

                byte[] plainTextBytes = Encoding.UTF8.GetBytes(textValue);

                PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                                passPhrase,
                                                                saltValueBytes,
                                                                hashAlgorithm,
                                                                passwordIterations);

                byte[] keyBytes = password.GetBytes(keySize / 8);


                RijndaelManaged symmetricKey = new RijndaelManaged();



                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                                                                 keyBytes,
                                                                 initVectorBytes);


                MemoryStream memoryStream = new MemoryStream();


                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                             encryptor,
                                                             CryptoStreamMode.Write);

                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);


                cryptoStream.FlushFinalBlock();


                byte[] cipherTextBytes = memoryStream.ToArray();


                memoryStream.Close();
                cryptoStream.Close();


                string cipherText = Convert.ToBase64String(cipherTextBytes);


                return cipherText;
            }
            return textValue;
        }


        public static string Decrypt(string encodedText, string decryptKey = null)
        {

            if (string.IsNullOrEmpty(decryptKey))
            {
                decryptKey = System.Configuration.ConfigurationSettings.AppSettings["EncryptDecryptKey"];
            }
            if (encodedText != null)
            {
                string passPhrase = "Pas5pr@se";
                int passwordIterations = 2;
                string hashAlgorithm = "SHA1";
                string initVector = "@1B2c3D4e5F6g7H8";
                int keySize = 128;

                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(decryptKey);


                string stringToDecrypt = encodedText.Replace(" ", "+");
                byte[] cipherTextBytes = Convert.FromBase64String(stringToDecrypt);

                PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                                passPhrase,
                                                                saltValueBytes,
                                                                hashAlgorithm,
                                                                passwordIterations);

                byte[] keyBytes = password.GetBytes(keySize / 8);


                RijndaelManaged symmetricKey = new RijndaelManaged();

                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                                                                 keyBytes,
                                                                 initVectorBytes);


                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);


                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                              decryptor,
                                                              CryptoStreamMode.Read);

                byte[] plainTextBytes = new byte[cipherTextBytes.Length];


                int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                           0,
                                                           plainTextBytes.Length);


                memoryStream.Close();
                cryptoStream.Close();

                string plainText = Encoding.UTF8.GetString(plainTextBytes,
                                                           0,
                                                           decryptedByteCount);


                return plainText;
            }
            return decryptKey;


        }
    }
}

