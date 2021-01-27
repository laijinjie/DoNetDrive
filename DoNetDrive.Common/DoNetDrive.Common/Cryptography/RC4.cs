using System;

namespace DoNetDrive.Common.Cryptography
{


    public class RC4
    {
        public static int VECTOR_LENGTH = 256;
        public static void RC4_Encrypt_CBC(ref byte[] Key, int key_len, ref byte[] IV, ref byte IVLen, ref byte[] data, int data_len)
        {
            byte[] prgaData;
            if ((data_len % IVLen) > 0)
            {
                byte[] tmp;
                var tmpLen = data_len + (IVLen - (data_len % IVLen));
                tmp = new byte[tmpLen - 1 + 1];
                Array.Copy(data, tmp, data_len);
                data_len = tmpLen;
                data = null;

                data = tmp;
            }

            prgaData = new byte[data_len + 1];
            // 密码初始化
            prga(ref Key, key_len, ref prgaData, data_len);
            byte[] tmpIV;
            tmpIV = (byte[])IV.Clone();
            var i = 0;
            var j = 0;
            do
            {
                for (j = 0; j <= IVLen - 1; j++)
                {
                    data[i + j] = (byte)(data[i + j] ^ tmpIV[j]);
                    data[i + j] = (byte)(prgaData[i + j] ^ data[i + j]);
                    tmpIV[j] = data[i + j];
                }

                i += IVLen;
            }
            while (i < data_len);

            prgaData = null;
        }


        public static bool RC4_Decrypt_CBC(ref byte[] Key, int key_len, ref byte[] IV, ref byte IVLen, ref byte[] data, int data_len)
        {
            byte[] prgaData;
            if (data_len % IVLen > 0)
                return false;

            prgaData = new byte[data_len + 1];
            // 密码初始化
            prga(ref Key, key_len, ref prgaData, data_len);
            byte[] tmpIV;
            tmpIV = new byte[IVLen - 1 + 1];
            var i = data_len - IVLen;
            var j = 0;

            do
            {
                if (i - IVLen >= 0)
                    Array.Copy(data, i - IVLen, tmpIV, 0, IVLen);
                else
                    Array.Copy(IV, tmpIV, IVLen);
                for (j = 0; j <= IVLen - 1; j++)
                {
                    data[i + j] = (byte)(prgaData[i + j] ^ data[i + j]);
                    data[i + j] = (byte)(data[i + j] ^ tmpIV[j]);
                }

                i -= IVLen;
            }
            while (i >= 0);

            prgaData = null;
            return true;
        }


        /// <summary>
        /// RC4 加密，使用指定的密钥 key，加密数据 data，并将加密后的数据放到 result 中
        /// </summary>
        public static void RC4_Encrypt(ref byte[] Key, int key_len, ref byte[] data, int data_len, ref byte[] result, ref int result_len)
        {
            int i;
            byte[] prgaData;

            prgaData = new byte[data_len + 1];

            prga(ref Key, key_len, ref prgaData, data_len);

            for (i = 0; i <= data_len - 1; i++)
                result[i] = (byte)(prgaData[i] ^ data[i]);
        }

        public static void RC4_Encrypt(ref byte[] Key, ref byte[] data)
        {
            int iLen = data.Length;
            RC4_Encrypt(ref Key, Key.Length, ref data, data.Length, ref data, ref iLen);
        }

        private static byte[] mRC4KeyData;
        private static int mRC4DataLen;

        /// <summary>
        /// 初始化RC4密钥
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="key_len"></param>
        public static void IniRC4Key(ref byte[] Key, int key_len, int data_len)
        {
            mRC4KeyData = new byte[data_len + 1];
            mRC4DataLen = data_len;
            prga(ref Key, key_len, ref mRC4KeyData, data_len);
        }


        /// <summary>
        /// RC4 加密，使用已初始化的密钥进行数据加密
        /// </summary>
        public static void RC4_Encrypt(ref byte[] data)
        {
            int i;

            for (i = 0; i < mRC4DataLen ; i++)
                data[i] = (byte)(mRC4KeyData[i] ^ data[i]);
        }


        /// <summary>
        /// RC4 加密，使用已初始化的密钥进行数据加密
        /// </summary>
        public static void RC4_Encrypt(ref byte[] data, int data_len)
        {
            int i;

            for (i = 0; i < data_len ; i++)
                data[i] = (byte)(mRC4KeyData[i] ^ data[i]);
        }

        public static void ksa(ref byte[] Key, int key_len, ref byte[] S)
        {
            var i = 0;
            var j = 0;
            byte temp = 0;

            for (i = 0; i <= VECTOR_LENGTH - 1; i++)
                S[i] = (byte)i;

            for (i = 0; i <= VECTOR_LENGTH - 1; i++)
            {
                j = (j + S[i] + Key[i % key_len]) % VECTOR_LENGTH;
                temp = S[j];
                S[j] = S[i];
                S[i] = temp;
            }
        }


        public static void prga(ref byte[] Key, int key_len, ref byte[] PRGA, int data_len)
        {
            byte[] S = new byte[VECTOR_LENGTH - 1 + 1];
            var i = 0;
            var j = 0;
            var counter = 0;
            byte temp = 0;
            int s1, s2;

            ksa(ref Key, key_len, ref S);

            for (counter = 0; counter <= data_len - 1; counter++)
            {
                i = (i + 1) % VECTOR_LENGTH;
                j = (j + S[i]) % VECTOR_LENGTH;
                temp = S[i];
                S[i] = S[j];
                S[j] = temp;


                s1 = S[i];
                s2 = S[j];
                PRGA[counter] = S[(s1 + s2) % VECTOR_LENGTH];
            }
        }
    }

}
