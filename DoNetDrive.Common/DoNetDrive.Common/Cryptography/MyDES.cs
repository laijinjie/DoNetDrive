using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace DoNetDrive.Common.Cryptography
{


    /// <summary>
    /// 使用DES密码算法加解密
    /// </summary>
    public sealed class MyDES
    {
        private string mIV;
        private string mKey;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MyDES()
        {
            mIV = "FCARD350";
            mKey = "DESFCARD";
        }


        /// <summary> 
        /// DES加密偏移量，必须是>=8位长的字符串  
        /// </summary>
        public string IV
        {
            get
            {
                return mIV;
            }
            set
            {
                mIV = value;
            }
        }

        /// <summary> 
        /// DES加密的私钥，必须是8位长的字符串  
        /// </summary>
        public string Key
        {
            get
            {
                return mKey;
            }

            set
            {
                mKey = value;
            }
        }

        /// <summary> 
        /// 对字节数组进行DES加密  
        /// </summary> 
        /// <param name="btData">待加密的字节数组</param> 
        /// <returns>加密后的字节数组</returns>
        public byte[] Encrypt(byte[] btData)
        {
            byte[] btKey = Encoding.Default.GetBytes(Key);
            byte[] btIV = Encoding.Default.GetBytes(IV);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();

            try
            {
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write);
                try
                {
                    cs.Write(btData, 0, btData.Length);
                    cs.FlushFinalBlock();
                }
                finally
                {
                    cs.Dispose();
                }

                return ms.ToArray();
            }
            catch
            {
                byte[] bt = new byte[0];
                return bt;
            }
        } // Encrypt  

        /// <summary> 
        /// 对字符串进行DES加密  
        /// </summary> 
        /// <param name="sourceString">待加密的字符串</param> 
        /// <returns>加密后的BASE64编码的字符串</returns>
        public string Encrypt(string sourceString)
        {
            try
            {
                return Convert.ToBase64String(Encrypt(Encoding.Default.GetBytes(sourceString)));
            }
            catch
            {
                return string.Empty;
            }
        } // Encrypt  


        /// <summary> 
        /// 对DES加密后的字节数组进行解密  
        /// </summary> 
        /// <param name="btData">待解密的字节数组</param> 
        /// <returns>解密后的字节数组</returns>
        public byte[] Decrypt(byte[] btData)
        {
            byte[] btKey = Encoding.Default.GetBytes(Key);
            byte[] btIV = Encoding.Default.GetBytes(IV);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            MemoryStream ms = new MemoryStream();
            try
            {
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write);
                try
                {
                    cs.Write(btData, 0, btData.Length);
                    cs.FlushFinalBlock();
                }
                finally
                {
                    cs.Dispose();
                }

                return ms.ToArray();
            }
            catch
            {
                byte[] bt = new byte[0];
                return bt;
            }
        } // Decrypt  


        /// <summary> 
        /// 对DES加密后的Base64字符串进行解密  
        /// </summary> 
        /// <param name="encryptedString">待解密的字符串</param> 
        /// <returns>解密后的字符串</returns>
        public string Decrypt(string encryptedString)
        {
            try
            {
                return Encoding.Default.GetString(Decrypt(Convert.FromBase64String(encryptedString)));
            }
            catch
            {
                return string.Empty;
            }
        } // Decrypt  

        /// <summary> 
        /// 对文件内容进行DES加密  
        /// </summary> 
        /// <param name="sourceFile">待加密的文件绝对路径</param> 
        /// <param name="destFile">加密后的文件保存的绝对路径</param>
        public void EncryptFile(string sourceFile, string destFile)
        {
            if (!File.Exists(sourceFile))
                throw new FileNotFoundException("指定的文件路径不存在！", sourceFile);
            byte[] btKey = Encoding.Default.GetBytes(Key);
            byte[] btIV = Encoding.Default.GetBytes(IV);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] btFile = File.ReadAllBytes(sourceFile);

            FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write);
            try
            {
                try
                {
                    CryptoStream cs = new CryptoStream(fs, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write);
                    try
                    {
                        cs.Write(btFile, 0, btFile.Length);
                        cs.FlushFinalBlock();
                    }
                    finally
                    {
                        cs.Dispose();
                    }
                }
                catch
                {
                }
                finally
                {
                    fs.Close();
                }
            }
            finally
            {
                fs.Dispose();
            }
        } // EncryptFile  

        /// <summary> 
        /// 对文件内容进行DES加密，加密后覆盖掉原来的文件  
        /// </summary> 
        /// <param name="sourceFile">待加密的文件的绝对路径</param>
        public void EncryptFile(string sourceFile)
        {
            EncryptFile(sourceFile, sourceFile);
        } // EncryptFile  

        /// <summary> 
        /// 对文件内容进行DES解密  
        /// </summary> 
        /// <param name="sourceFile">待解密的文件绝对路径</param> 
        /// <param name="destFile">解密后的文件保存的绝对路径</param>
        public void DecryptFile(string sourceFile, string destFile)
        {
            if (!File.Exists(sourceFile))
                throw new FileNotFoundException("指定的文件路径不存在！", sourceFile);
            byte[] btKey = Encoding.Default.GetBytes(Key);
            byte[] btIV = Encoding.Default.GetBytes(IV);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] btFile = File.ReadAllBytes(sourceFile);

            FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write);
            try
            {
                try
                {
                    CryptoStream cs = new CryptoStream(fs, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write);
                    try
                    {
                        cs.Write(btFile, 0, btFile.Length);
                        cs.FlushFinalBlock();
                    }
                    finally
                    {
                        cs.Dispose();
                    }
                }
                catch
                {
                }
                finally
                {
                    fs.Close();
                }
            }
            finally
            {
                fs.Dispose();
            }
        } // DecryptFile  

        /// <summary> 
        /// 对文件内容进行DES解密，加密后覆盖掉原来的文件  
        /// </summary> 
        /// <param name="sourceFile">待解密的文件的绝对路径</param>
        public  void DecryptFile(string sourceFile)
        {
            DecryptFile(sourceFile, sourceFile);
        } // DecryptFile  
    } // DES

}
