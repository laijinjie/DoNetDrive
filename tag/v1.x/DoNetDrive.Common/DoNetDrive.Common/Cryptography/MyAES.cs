using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DoNetDrive.Common.Cryptography
{

    /// <summary>
    /// 加密工具类
    /// </summary>
    public class MyAES
    {
        /// <summary>
        /// 使用AES加密数据
        /// </summary>
        /// <param name="bKey"></param>
        /// <param name="bIV"></param>
        /// <param name="bData"></param>
        /// <param name="pad">PaddingMode</param>
        /// <returns></returns>
        public static byte[] Encryptor(byte[] bKey, byte[] bIV, byte[] bData, PaddingMode pad)
        {
            RijndaelManaged aseDEL;
            aseDEL = new RijndaelManaged();

            {
                var withBlock = aseDEL;
                withBlock.Padding = pad;
                withBlock.Mode = CipherMode.CBC; // 带有初始化向量
                withBlock.IV = bIV;
                withBlock.Key = bKey;
                using (ICryptoTransform cEncryptor = withBlock.CreateEncryptor())
                {
                    return cEncryptor.TransformFinalBlock(bData, 0, bData.Length);
                }
            }
        }

        /// <summary>
        /// 使用AES加密数据
        /// </summary>
        /// <param name="bKey"></param>
        /// <param name="bIV"></param>
        /// <param name="bData"></param>
        /// <returns></returns>
        public static byte[] Encryptor(byte[] bKey, byte[] bIV, byte[] bData)
        {
            return Encryptor(bKey, bIV, bData, PaddingMode.PKCS7);
        }

        /// <summary>
        ///  AES加密UTF8字符串 并返回bas64编码的密文
        ///  </summary>
        ///  <param name="plainStr">要加密的字符串</param>
        ///  <param name="base64_Key">密钥 32字节</param>
        ///  <param name="base64_IV">向量 16字节</param>
        ///  <returns></returns>
        public static string Encryptor( string plainStr,  string base64_Key,  string base64_IV)
        {
            var UserKey = Convert.FromBase64String(base64_Key);
            var UserIV = Convert.FromBase64String(base64_IV);
            return Encryptor( plainStr,  UserKey,  UserIV);
        }

        /// <summary>
        /// AES加密字节
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte[] Encryptor(byte[] b,string key, string iv)
        {
            AesCryptoServiceProvider provider = new AesCryptoServiceProvider();
            provider.Key = Convert.FromBase64String(key);
            provider.IV = Convert.FromBase64String(iv);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, provider.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(b, 0, b.Length);
                    cs.FlushFinalBlock();

                    return ms.ToArray();
                }
            }
        }

       


        /// <summary>
        ///  AES加密UTF8字符串，并返回base64
        ///  </summary>
        ///  <param name="plainStr">要加密的字符串</param>
        ///  <param name="bKey">密钥 32字节</param>
        ///  <param name="bIV">向量 16字节</param>
        ///  <returns></returns>
        public static string Encryptor( string plainStr,  byte[] bKey,  byte[] bIV)
        {
            var byteArray = Encoding.UTF8.GetBytes(plainStr);

            byteArray = Encryptor(bKey, bIV, byteArray, PaddingMode.PKCS7);
            var encrypt = Convert.ToBase64String(byteArray);
            //var aes = Rijndael.Create();
            //aes.KeySize = 256;
            //try
            //{
            //    var mStream = new MemoryStream();
            //    var cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write);
            //    cStream.Write(byteArray, 0, byteArray.Length);
            //    cStream.FlushFinalBlock();
            //    encrypt = Convert.ToBase64String(mStream.ToArray());

            //    cStream.Close();
            //    cStream.Dispose();
            //    mStream.Close();
            //    mStream.Dispose();
            //}
            //catch (Exception ex)
            //{
            //}
            //aes.Clear();
            //aes = null/* TODO Change to default(_) if this is not a reference type */;

            return encrypt;
        }

        /// <summary>
        /// 使用AES解密数据
        /// </summary>
        /// <param name="bKey"></param>
        /// <param name="bIV"></param>
        /// <param name="bData"></param>
        /// <returns></returns>
        public static byte[] Decryptor(byte[] bKey, byte[] bIV, byte[] bData)
        {
            return Decryptor(bKey, bIV, bData, PaddingMode.PKCS7);
        }

        /// <summary>
        /// 使用AES解密数据
        /// </summary>
        /// <param name="bKey"></param>
        /// <param name="bIV"></param>
        /// <param name="bData"></param>
        /// <param name="pad">PaddingMode</param>
        /// <returns></returns>
        public static byte[] Decryptor(byte[] bKey, byte[] bIV, byte[] bData, PaddingMode pad)
        {
            RijndaelManaged aseDEL;
            aseDEL = new RijndaelManaged();

            {
                var withBlock = aseDEL;
                withBlock.Padding = pad;
                withBlock.Mode = CipherMode.CBC; // 带有初始化向量
                withBlock.IV = bIV;
                withBlock.Key = bKey;
                using (ICryptoTransform cDecryptor = withBlock.CreateDecryptor())
                {
                    return cDecryptor.TransformFinalBlock(bData, 0, bData.Length);
                }
            }
        }


        /// <summary>
        ///  AES 解密,并返回 UTF8字符串
        ///  </summary>
        ///  <param name="encryptStr">要解密字符串</param>
        ///  <param name="base64_Key">密钥 32字节</param>
        ///  <param name="base64_IV">向量 16字节</param>
        ///  <returns></returns>
        public static string Decryptor( string encryptStr,  string base64_Key,  string base64_IV)
        {
            var UserKey = Convert.FromBase64String(base64_Key);
            var UserIV = Convert.FromBase64String(base64_IV);
            return Decryptor( encryptStr,  UserKey,  UserIV);
        }

        /// <summary>
        ///  AES 解密,并返回 UTF8字符串
        ///  </summary>
        ///  <param name="encryptStr">要解密字符串</param>
        ///  <param name="bKey">密钥 32字节</param>
        ///  <param name="bIV">向量 16字节</param>
        ///  <returns></returns>
        public static string Decryptor( string encryptStr,  byte[] bKey,  byte[] bIV)
        {
            byte[] byteArray;
            var decrypt = string.Empty;

            try
            {
                byteArray = Convert.FromBase64String(encryptStr);

                byteArray = Decryptor(bKey, bIV, byteArray, PaddingMode.PKCS7);
                decrypt = Encoding.UTF8.GetString(byteArray);

            }
            catch (Exception ex)
            {
            }
            
            return decrypt;
        }
    }

}
