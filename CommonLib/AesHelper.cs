using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CommonLib
{
    public class AesHelper
    {
        /// <summary>
        /// 获取密钥
        /// </summary>
        private static string Key
        {
            get { return @")O[NB]6,YF}+efcaj{+oESb9d8>Z'e9M"; }
        }

        /// <summary>
        /// 获取向量
        /// </summary>
        private static string Iv
        {
            get { return @"L+\~f4,Ir)b$=pkf"; }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <returns>密文</returns>
        private static string AesEncrypt(string plainStr)
        {
            var bKey = Encoding.UTF8.GetBytes(Key);
            var bIv = Encoding.UTF8.GetBytes(Iv);
            var byteArray = Encoding.UTF8.GetBytes(plainStr);

            string encrypt = null;
            var aes = Rijndael.Create();
            try
            {
                using (var mStream = new MemoryStream())
                {
                    using (var cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIv), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            catch { }
            aes.Clear();

            return encrypt;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <param name="returnNull">加密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>密文</returns>
        public static string Encrypt(string plainStr, bool returnNull = true)
        {
            var encrypt = AesEncrypt(plainStr);
            return returnNull ? encrypt : (encrypt == null ? String.Empty : encrypt);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <returns>明文</returns>
        private static string AesDecrypt(string encryptStr)
        {
            var bKey = Encoding.UTF8.GetBytes(Key);
            var bIv = Encoding.UTF8.GetBytes(Iv);
            var byteArray = Convert.FromBase64String(encryptStr);

            string decrypt = null;
            var aes = Rijndael.Create();

            try
            {
                using (var mStream = new MemoryStream())
                {
                    using (var cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIv), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
            catch { }
            aes.Clear();

            return decrypt;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <param name="returnNull">解密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encryptStr, bool returnNull = true)
        {
            var decrypt = AesDecrypt(encryptStr);
            return returnNull ? decrypt : (decrypt == null ? String.Empty : decrypt);
        }

    }
}
