using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AIA.Life.Repository.AIAEntity;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Configuration;

namespace AIA.Presentation.AVOLife
{
    public class LoginUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public static string GetUserName()
        {
            string UserName = HttpContext.Current.User.Identity.Name;
            if (UserName.Contains("\\"))
            {
                UserName = UserName.Substring(UserName.LastIndexOf("\\") + 1);
                //  UserName = "70005090";
            }
            return UserName;
        }
        public static string GetUserName(Guid? userID)
        {
            string userName = "";
            if (userID.HasValue)
            {
                AVOAIALifeEntities entities = new AVOAIALifeEntities();
                userName = entities.tblUserDetails.Where(a => a.UserID == userID).FirstOrDefault().LoginID;
            }
            return userName;
        }
        //public static Guid GetUserId()
        //{
        //    if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated && LoginUser.GetUserName() != "")
        //    {
        //        FutureGeneraliEntities entities = new FutureGeneraliEntities();
        //        string UserName = LoginUser.GetUserName();
        //        var user = entities.tblUserDetails.FirstOrDefault(a => a.LoginID == UserName);
        //        if (user == null)
        //        {
        //            return new Guid();
        //        }
        //        return new Guid(user.UserID.ToString());
        //    }
        //    else
        //    {
        //        return new Guid();
        //    }
        //}


        //obul added below code at 20-09-2013 for External Service.
        public static Guid GetCurrentUserId(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                AVOAIALifeEntities entities = new AVOAIALifeEntities();
                var user = entities.Users.FirstOrDefault(a => a.UserName == userName);
                if (user == null)
                {
                    return new Guid();
                }
                return user.UserId;
            }
            else
            {
                return new Guid();
            }
        }

        public static Guid GetUserId()
        {
            if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated && LoginUser.GetUserName() != "")
            {

                try
                {
                    Guid userId = Guid.Empty;

                    HttpCookie UserCookie = new HttpCookie("User");
                    UserCookie = HttpContext.Current.Request.Cookies["User"];
                    if (UserCookie != null)
                    {

                        string user = UserCookie.Value.ToString();
                        user = user.DecryptData();
                        userId = new Guid(user);
                    }
                    else
                    {
                        AVOAIALifeEntities entities = new AVOAIALifeEntities();
                        string UserName = LoginUser.GetUserName();
                        var user = entities.tblUserDetails.FirstOrDefault(a => a.LoginID == UserName);
                        if (user == null)
                        {
                            return new Guid();
                        }
                        userId = new Guid(user.UserID.ToString());
                    }
                    return userId;
                }
                catch (Exception ex)
                {
                    return new Guid();
                }
            }
            else
            {
                return new Guid();
            }
        }
       
    }
    public static class DataTypeConvExtention
    {
        public static string DecryptData(this string encryptedValue)
        {
            if (!string.IsNullOrEmpty(encryptedValue))
            {
                encryptedValue = HttpContext.Current.Server.UrlDecode(encryptedValue);
                encryptedValue = (EncryptDecrypt.Decrypt(encryptedValue));
            }
            else
            {
                encryptedValue = string.Empty;
            }
            return encryptedValue;

        }

        public static string EncryptData(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                 value = EncryptDecrypt.Encrypt(value);
                value = System.Web.HttpUtility.UrlEncode(value);
            }
            else
            {
                value = string.Empty;
            }
            return value;
        }
        public static string ToddMMyyyyString(this DateTime dateValue)
        {
            string dateFormat = String.Format("{0:dd/MM/yyyy}", dateValue);
            return dateFormat;
        }
        public static DateTime ToDate(this string Date)
        {
            DateTime d = new DateTime();
            DateTime.TryParse(Date, out d);
            return d;
        }
    }
    public static class EncryptDecrypt
    {


        public static string Encrypt(string textValue, string encryptKey = null)
        {
            if (string.IsNullOrEmpty(encryptKey))
            {
                encryptKey = ConfigurationManager.AppSettings["EncryptDecryptKey"];
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
                decryptKey = ConfigurationManager.AppSettings["EncryptDecryptKey"];
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

   public static class ValidateFileType
    {
        public static bool IsAllowedFileType(this string FileName, Stream allBytes)
        {
            //BinaryReader reader = new BinaryReader(new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            BinaryReader reader = new BinaryReader(allBytes);
           // reader.BaseStream.Position = 0x0; // The offset you are reading the data from  
            byte[] data = reader.ReadBytes(0x10); // Read 16 bytes into an array  
            string data_as_hex = BitConverter.ToString(data);
            reader.Close();

            // substring to select first 11 characters from hexadecimal array  
            string signature = data_as_hex.Substring(0, 11);
            signature = signature.Replace("-", " ");
            bool isAllowed = false;
            var AllowedFileTypes = new string[]
             {
                "25 50 44 46",//pdf
                //"D0 CF 11 E0",//doc xls ppt vsd
                //"50 4B 03 04",//docx xlsx pptx
                "FF D8 FF E0","FF D8 FF E1","FF D8 FF E8",//jpeg jpg 
                "89 50 4E 47",//png
               // "42 4D"//bmp
             };
            isAllowed = AllowedFileTypes.Contains(signature);

            return isAllowed;
        }
    }
}

