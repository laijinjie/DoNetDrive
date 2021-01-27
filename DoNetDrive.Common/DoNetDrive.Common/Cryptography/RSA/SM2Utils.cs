
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Common.Cryptography.RSA
{
    public class SM2Utils
    {
        /// <summary>
        /// 创建密钥对
        /// </summary>
        /// <param name="sPublicKey_Hex">公钥</param>
        /// <param name="sPrivateKey_Hex">私钥</param>
        public static void GenerateKeyPair(out string sPublicKey_Hex, out string sPrivateKey_Hex)
        {
            SM2 sm2 = SM2.Instance;
            AsymmetricCipherKeyPair key = sm2.ecc_key_pair_generator.GenerateKeyPair();
            ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)key.Private;
            ECPublicKeyParameters ecpub = (ECPublicKeyParameters)key.Public;
            //公钥
            BigInteger privateKey = ecpriv.D;
            //私钥
            ECPoint publicKey = ecpub.Q;

            sPublicKey_Hex = Encoding.Default.GetString(Hex.Encode(publicKey.GetEncoded())).ToUpper();
            sPrivateKey_Hex = Encoding.Default.GetString(Hex.Encode(privateKey.ToByteArray())).ToUpper();
        }


        /// <summary>
        /// 使用公钥进行加密
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="data">需要加密的数据块</param>
        /// <returns>Hex密文字符串</returns>
        public static String Encrypt(byte[] publicKey, byte[] data)
        {
            if (null == publicKey || publicKey.Length == 0)
            {
                return null;
            }
            if (data == null || data.Length == 0)
            {
                return null;
            }

            byte[] source = new byte[data.Length];
            Array.Copy(data, 0, source, 0, data.Length);

            Cipher cipher = new Cipher();
            SM2 sm2 = SM2.Instance;

            ECPoint userKey = sm2.ecc_curve.DecodePoint(publicKey);

            ECPoint c1 = cipher.Init_enc(sm2, userKey);
            cipher.Encrypt(source);

            byte[] c3 = new byte[32];
            cipher.Dofinal(c3);

            String sc1 = Encoding.Default.GetString(Hex.Encode(c1.GetEncoded()));
            String sc2 = Encoding.Default.GetString(Hex.Encode(source));
            String sc3 = Encoding.Default.GetString(Hex.Encode(c3));

            return (sc1 + sc2 + sc3).ToUpper();
        }

        /// <summary>
        /// 使用私钥进行解密
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="encryptedData">待解密的密文</param>
        /// <returns>字节数组存储的明文</returns>
        public static byte[] Decrypt(byte[] privateKey, byte[] encryptedData)
        {
            if (null == privateKey || privateKey.Length == 0)
            {
                return null;
            }
            if (encryptedData == null || encryptedData.Length == 0)
            {
                return null;
            }
            int c2Len = encryptedData.Length - 97;


            //String data = Encoding.Default.GetString(Hex.Encode(encryptedData));
            //byte[] c1Bytes_h = Hex.Decode(Encoding.Default.GetBytes(data.Substring(0, 130)));
            //byte[] c2_h = Hex.Decode(Encoding.Default.GetBytes(data.Substring(130, 2 * c2Len)));
            //byte[] c3_h = Hex.Decode(Encoding.Default.GetBytes(data.Substring(130 + 2 * c2Len, 64)));



            byte[] c1Bytes = new byte[65]; 
            Array.Copy(encryptedData, 0, c1Bytes, 0, 65);
            
            byte[] c2 = new byte[c2Len];
            Array.Copy(encryptedData, 65, c2,0, c2Len);

            byte[] c3 = new byte[32];
            Array.Copy(encryptedData, 65+ c2Len, c3, 0, 32);


            SM2 sm2 = SM2.Instance;
            BigInteger userD = new BigInteger(1, privateKey);

            ECPoint c1 = sm2.ecc_curve.DecodePoint(c1Bytes);
            Cipher cipher = new Cipher();
            cipher.Init_dec(userD, c1);//生成随机数的计算出的椭圆曲线点
            cipher.Decrypt(c2);//密文数据
            cipher.Dofinal(c3);//SM3的摘要值

            return c2;
        }

        //[STAThread]
        //public static void Main()
        //{
        //    GenerateKeyPair();

        //    String plainText = "ererfeiisgod";
        //    byte[] sourceData = Encoding.Default.GetBytes(plainText);

        //    //下面的秘钥可以使用generateKeyPair()生成的秘钥内容  
        //    // 国密规范正式私钥  
        //    String prik = "3690655E33D5EA3D9A4AE1A1ADD766FDEA045CDEAA43A9206FB8C430CEFE0D94";
        //    // 国密规范正式公钥  
        //    String pubk = "04F6E0C3345AE42B51E06BF50B98834988D54EBC7460FE135A48171BC0629EAE205EEDE253A530608178A98F1E19BB737302813BA39ED3FA3C51639D7A20C7391A";

        //    System.Console.Out.WriteLine("加密: ");
        //    String cipherText = SM2Utils.Encrypt(Hex.Decode(pubk), sourceData);
        //    System.Console.Out.WriteLine(cipherText);
        //    System.Console.Out.WriteLine("解密: ");
        //    plainText = Encoding.Default.GetString(SM2Utils.Decrypt(Hex.Decode(prik), Hex.Decode(cipherText)));
        //    System.Console.Out.WriteLine(plainText);

        //    Console.ReadLine();
        //}
    }
}