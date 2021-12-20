using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoNetDrive.Common.Cryptography
{
    using System;
    using DoNetDrive.Common.Extensions;
    using Microsoft.VisualBasic;

    public class DES_C
    {
        public static System.Text.Encoding gEncoding = System.Text.Encoding.ASCII; // 本系统使用的编码
                                                                                   // permuted choice table (PC1) PC1_Table(56)
        private static byte[] PC1_Table = new byte[] {
        57,
        49,
        41,
        33,
        25,
        17,
        9,
        1,
        58,
        50,
        42,
        34,
        26,
        18,
        10,
        2,
        59,
        51,
        43,
        35,
        27,
        19,
        11,
        3,
        60,
        52,
        44,
        36,
        63,
        55,
        47,
        39,
        31,
        23,
        15,
        7,
        62,
        54,
        46,
        38,
        30,
        22,
        14,
        6,
        61,
        53,
        45,
        37,
        29,
        21,
        13,
        5,
        28,
        20,
        12,
        4
    };
        // permuted choice key (PC2) PC2_Table(48)
        private static byte[] PC2_Table = new byte[] {
        14,
        17,
        11,
        24,
        1,
        5,
        3,
        28,
        15,
        6,
        21,
        10,
        23,
        19,
        12,
        4,
        26,
        8,
        16,
        7,
        27,
        20,
        13,
        2,
        41,
        52,
        31,
        37,
        47,
        55,
        30,
        40,
        51,
        45,
        33,
        48,
        44,
        49,
        39,
        56,
        34,
        53,
        46,
        42,
        50,
        36,
        29,
        32
    };
        // number left rotations of pc1  Shift_Table(16)
        private static byte[] Shift_Table = new byte[] {
        1,
        1,
        2,
        2,
        2,
        2,
        2,
        2,
        1,
        2,
        2,
        2,
        2,
        2,
        2,
        1
    };
        // initial permustatictation (IP) IP_Table(64)
        private static byte[] IP_Table = new byte[] {
        58,
        50,
        42,
        34,
        26,
        18,
        10,
        2,
        60,
        52,
        44,
        36,
        28,
        20,
        12,
        4,
        62,
        54,
        46,
        38,
        30,
        22,
        14,
        6,
        64,
        56,
        48,
        40,
        32,
        24,
        16,
        8,
        57,
        49,
        41,
        33,
        25,
        17,
        9,
        1,
        59,
        51,
        43,
        35,
        27,
        19,
        11,
        3,
        61,
        53,
        45,
        37,
        29,
        21,
        13,
        5,
        63,
        55,
        47,
        39,
        31,
        23,
        15,
        7
    };
        // expansion operation matrix (E) E_Table(48)
        private static byte[] E_Table = new byte[] {
        32,
        1,
        2,
        3,
        4,
        5,
        4,
        5,
        6,
        7,
        8,
        9,
        8,
        9,
        10,
        11,
        12,
        13,
        12,
        13,
        14,
        15,
        16,
        17,
        16,
        17,
        18,
        19,
        20,
        21,
        20,
        21,
        22,
        23,
        24,
        25,
        24,
        25,
        26,
        27,
        28,
        29,
        28,
        29,
        30,
        31,
        32,
        1
    };
        // The (in)famous S-boxes  S_Box(8)(4)(16)
        private static byte[,,] S_Box = new byte[,,] {
        {
            {
                14,
                4,
                13,
                1,
                2,
                15,
                11,
                8,
                3,
                10,
                6,
                12,
                5,
                9,
                0,
                7
            },
            {
                0,
                15,
                7,
                4,
                14,
                2,
                13,
                1,
                10,
                6,
                12,
                11,
                9,
                5,
                3,
                8
            },
            {
                4,
                1,
                14,
                8,
                13,
                6,
                2,
                11,
                15,
                12,
                9,
                7,
                3,
                10,
                5,
                0
            },
            {
                15,
                12,
                8,
                2,
                4,
                9,
                1,
                7,
                5,
                11,
                3,
                14,
                10,
                0,
                6,
                13
            }
        },
        {
            {
                15,
                1,
                8,
                14,
                6,
                11,
                3,
                4,
                9,
                7,
                2,
                13,
                12,
                0,
                5,
                10
            },
            {
                3,
                13,
                4,
                7,
                15,
                2,
                8,
                14,
                12,
                0,
                1,
                10,
                6,
                9,
                11,
                5
            },
            {
                0,
                14,
                7,
                11,
                10,
                4,
                13,
                1,
                5,
                8,
                12,
                6,
                9,
                3,
                2,
                15
            },
            {
                13,
                8,
                10,
                1,
                3,
                15,
                4,
                2,
                11,
                6,
                7,
                12,
                0,
                5,
                14,
                9
            }
        },
        {
            {
                10,
                0,
                9,
                14,
                6,
                3,
                15,
                5,
                1,
                13,
                12,
                7,
                11,
                4,
                2,
                8
            },
            {
                13,
                7,
                0,
                9,
                3,
                4,
                6,
                10,
                2,
                8,
                5,
                14,
                12,
                11,
                15,
                1
            },
            {
                13,
                6,
                4,
                9,
                8,
                15,
                3,
                0,
                11,
                1,
                2,
                12,
                5,
                10,
                14,
                7
            },
            {
                1,
                10,
                13,
                0,
                6,
                9,
                8,
                7,
                4,
                15,
                14,
                3,
                11,
                5,
                2,
                12
            }
        },
        {
            {
                7,
                13,
                14,
                3,
                0,
                6,
                9,
                10,
                1,
                2,
                8,
                5,
                11,
                12,
                4,
                15
            },
            {
                13,
                8,
                11,
                5,
                6,
                15,
                0,
                3,
                4,
                7,
                2,
                12,
                1,
                10,
                14,
                9
            },
            {
                10,
                6,
                9,
                0,
                12,
                11,
                7,
                13,
                15,
                1,
                3,
                14,
                5,
                2,
                8,
                4
            },
            {
                3,
                15,
                0,
                6,
                10,
                1,
                13,
                8,
                9,
                4,
                5,
                11,
                12,
                7,
                2,
                14
            }
        },
        {
            {
                2,
                12,
                4,
                1,
                7,
                10,
                11,
                6,
                8,
                5,
                3,
                15,
                13,
                0,
                14,
                9
            },
            {
                14,
                11,
                2,
                12,
                4,
                7,
                13,
                1,
                5,
                0,
                15,
                10,
                3,
                9,
                8,
                6
            },
            {
                4,
                2,
                1,
                11,
                10,
                13,
                7,
                8,
                15,
                9,
                12,
                5,
                6,
                3,
                0,
                14
            },
            {
                11,
                8,
                12,
                7,
                1,
                14,
                2,
                13,
                6,
                15,
                0,
                9,
                10,
                4,
                5,
                3
            }
        },
        {
            {
                12,
                1,
                10,
                15,
                9,
                2,
                6,
                8,
                0,
                13,
                3,
                4,
                14,
                7,
                5,
                11
            },
            {
                10,
                15,
                4,
                2,
                7,
                12,
                9,
                5,
                6,
                1,
                13,
                14,
                0,
                11,
                3,
                8
            },
            {
                9,
                14,
                15,
                5,
                2,
                8,
                12,
                3,
                7,
                0,
                4,
                10,
                1,
                13,
                11,
                6
            },
            {
                4,
                3,
                2,
                12,
                9,
                5,
                15,
                10,
                11,
                14,
                1,
                7,
                6,
                0,
                8,
                13
            }
        },
        {
            {
                4,
                11,
                2,
                14,
                15,
                0,
                8,
                13,
                3,
                12,
                9,
                7,
                5,
                10,
                6,
                1
            },
            {
                13,
                0,
                11,
                7,
                4,
                9,
                1,
                10,
                14,
                3,
                5,
                12,
                2,
                15,
                8,
                6
            },
            {
                1,
                4,
                11,
                13,
                12,
                3,
                7,
                14,
                10,
                15,
                6,
                8,
                0,
                5,
                9,
                2
            },
            {
                6,
                11,
                13,
                8,
                1,
                4,
                10,
                7,
                9,
                5,
                0,
                15,
                14,
                2,
                3,
                12
            }
        },
        {
            {
                13,
                2,
                8,
                4,
                6,
                15,
                11,
                1,
                10,
                9,
                3,
                14,
                5,
                0,
                12,
                7
            },
            {
                1,
                15,
                13,
                8,
                10,
                3,
                7,
                4,
                12,
                5,
                6,
                11,
                0,
                14,
                9,
                2
            },
            {
                7,
                11,
                4,
                1,
                9,
                12,
                14,
                2,
                0,
                6,
                10,
                13,
                15,
                3,
                5,
                8
            },
            {
                2,
                1,
                14,
                7,
                4,
                10,
                8,
                13,
                15,
                12,
                9,
                0,
                3,
                5,
                6,
                11
            }
        }
    };
        // 32-bit permutation function P used on the output of the S-boxes  P_Table(32)
        private static byte[] P_Table = new byte[] {
        16,
        7,
        20,
        21,
        29,
        12,
        28,
        17,
        1,
        15,
        23,
        26,
        5,
        18,
        31,
        10,
        2,
        8,
        24,
        14,
        32,
        27,
        3,
        9,
        19,
        13,
        30,
        6,
        22,
        11,
        4,
        25
    };
        // final permutation IP^-1 IPR_Table(64)
        private static byte[] IPR_Table = new byte[] {
        40,
        8,
        48,
        16,
        56,
        24,
        64,
        32,
        39,
        7,
        47,
        15,
        55,
        23,
        63,
        31,
        38,
        6,
        46,
        14,
        54,
        22,
        62,
        30,
        37,
        5,
        45,
        13,
        53,
        21,
        61,
        29,
        36,
        4,
        44,
        12,
        52,
        20,
        60,
        28,
        35,
        3,
        43,
        11,
        51,
        19,
        59,
        27,
        34,
        2,
        42,
        10,
        50,
        18,
        58,
        26,
        33,
        1,
        41,
        9,
        49,
        17,
        57,
        25
    };


        private byte[,,] szSubKeys = new byte[2, 16, 48]; // 储存2个16组48位密钥,第2个用于3DES
        private byte[] szCiphertextRaw = new byte[64]; // 储存二进制密文(64个Bits) 
        private byte[] szPlaintextRaw = new byte[64]; // 储存二进制密文(64个Bits)
        private byte[] szCiphertextInBytes = new byte[8]; // 储存8位密文
        private byte[] szPlaintextInBytes = new byte[8]; // 储存8位明文字符串

        private byte[] szCiphertextInBinary = new byte[64]; // 储存二进制密文(64个Bits) char '0','1',最后一位存'\0'
        private byte[] szCiphertextInHex = new byte[16]; // 储存十六进制密文,最后一位存'\0'
        private byte[] szPlaintext = new byte[8]; // 储存8位明文字符串,最后一位存'\0'

        private byte[] szFCiphertextAnyLength;   // 任意长度密文
        private Int32 szFCiphertextAnyLengthMax;
        private byte[] szFPlaintextAnyLength; // 任意长度明文字符串
        private Int32 szFPlaintextAnyLengthMax;

        public DES_C()
        {
        }

        /// <summary>
        /// 设置初始化密钥
        /// </summary>
        /// <remarks></remarks>
        public DES_C(byte[] bKey)
        {
            // 设置密钥
            byte[] bData = new byte[8];

            if (bKey.Length < 7)
                Array.Copy(bKey, bData, bKey.Length);
            else
                Array.Copy(bKey, bData, 8);

            InitializeKey(bData, 0);
        }

        /// <summary>
        /// 设置初始化密钥
        /// </summary>
        /// <param name="sKey"></param>
        /// <remarks></remarks>
        public DES_C(string sKey)
        {
            // 设置密钥
            byte[] bData = new byte[8];
            byte[] bKey = System.Text.Encoding.Default.GetBytes(sKey);

            if (bKey.Length < 7)
                Array.Copy(bKey, bData, bKey.Length);
            else
                Array.Copy(bKey, bData, 8);
            InitializeKey(bData, 0);
        }

        /// <summary>
        /// 加密任意长度字符串
        /// 结果:函数将加密后结果存放于缓存,用户通过属性CiphertextAnyLength得到
        /// </summary>
        /// <param name="srcBytes">任意长度字节数组</param>
        /// <param name="srcLength">长度</param>
        /// <param name="keyN">使用Key的序号0-1</param>
        /// <remarks></remarks>
        public void EncryptAnyLength(byte[] srcBytes, Int32 srcLength, Int32 keyN)
        {
            Int32 iParts, iResidue;
            byte[] szTmp8Byte = new byte[8];

            if ((srcLength == 8))
            {
                szFCiphertextAnyLength = new byte[8];
                szFCiphertextAnyLengthMax = 8;

                EncryptData(srcBytes, keyN);

                Array.Copy(szCiphertextInBytes, 0, szFCiphertextAnyLength, 0, 8);
            }
            else if ((srcLength < 8))
            {
                szFCiphertextAnyLength = new byte[8];
                szFCiphertextAnyLengthMax = 8;

                Array.Copy(srcBytes, 0, szTmp8Byte, 0, srcLength);
                EncryptData(szTmp8Byte, keyN);
                Array.Copy(szCiphertextInBytes, 0, szFCiphertextAnyLength, 0, 8);
            }
            else if ((srcLength > 8))
            {
                iParts = srcLength >> 3;
                iResidue = srcLength % 8;

                // 计算加密后的数据长度
                szFCiphertextAnyLengthMax = iParts * 8;
                if ((!(iResidue == 0)))
                    szFCiphertextAnyLengthMax = szFCiphertextAnyLengthMax + 8;
                // 初始化加密后的存储空间
                szFCiphertextAnyLength = new byte[szFCiphertextAnyLengthMax - 1 + 1];

                for (Int32 i = 0; i <= iParts - 1; i++)
                {
                    Array.Copy(srcBytes, (i << 3), szTmp8Byte, 0, 8);
                    EncryptData(szTmp8Byte, keyN);
                    Array.Copy(szCiphertextInBytes, 0, szFCiphertextAnyLength, (i << 3), 8);
                }

                // 初始化数组
                Array.Clear(szTmp8Byte, 0, szTmp8Byte.Length);

                if ((!(iResidue == 0)))
                {
                    Array.Copy(srcBytes, (iParts << 3), szTmp8Byte, 0, iResidue);
                    EncryptData(szTmp8Byte, keyN);
                    Array.Copy(szCiphertextInBytes, 0, szFCiphertextAnyLength, (iParts << 3), 8);
                }
            }
        }

        /// <summary>
        /// 解密任意长度字节数组
        /// 函数将加密后结果存放于缓存，通过属性PlaintextAnyLength得到
        /// </summary>
        /// <param name="srcBytes">任意长度字节数组</param>
        /// <param name="srcLength">长度</param>
        /// <param name="keyN">使用Key的序号0-1</param>
        /// <remarks></remarks>
        public void DecryptAnyLength(byte[] srcBytes, Int32 srcLength, Int32 keyN)
        {
            // 解码的数据必须是8的倍数，否则就无法解码
            Int32 iParts, iResidue;
            byte[] szTmp8Byte = new byte[8];

            if ((srcLength == 8))
            {
                szFPlaintextAnyLength = new byte[8];
                szFPlaintextAnyLengthMax = 8;

                DecryptData(srcBytes, keyN);
                Array.Copy(szPlaintextInBytes, 0, szFPlaintextAnyLength, 0, 8);
            }
            else if ((srcLength < 8))
            {
                szFPlaintextAnyLength = new byte[8];
                szFPlaintextAnyLengthMax = 8;

                Array.Copy(srcBytes, 0, szTmp8Byte, 0, srcLength);
                DecryptData(szTmp8Byte, keyN);
                Array.Copy(szPlaintextInBytes, 0, szFPlaintextAnyLength, 0, 8);
            }
            else if ((srcLength > 8))
            {
                iParts = srcLength >> 3;
                iResidue = srcLength % 8;

                // 计算解密后的数据长度
                szFPlaintextAnyLengthMax = iParts * 8;
                if ((!(iResidue == 0)))
                    szFPlaintextAnyLengthMax = szFPlaintextAnyLengthMax + 8;
                // 初始化解密后的存储空间
                szFPlaintextAnyLength = new byte[szFPlaintextAnyLengthMax - 1 + 1];

                for (Int32 i = 0; i <= iParts - 1; i++)
                {
                    Array.Copy(srcBytes, (i << 3), szTmp8Byte, 0, 8);
                    DecryptData(szTmp8Byte, keyN);
                    Array.Copy(szPlaintextInBytes, 0, szFPlaintextAnyLength, (i << 3), 8);
                }

                if ((!(iResidue == 0)))
                {

                    // 初始化数组
                    Array.Clear(szTmp8Byte, 0, szTmp8Byte.Length);

                    Array.Copy(srcBytes, (iParts << 3), szTmp8Byte, 0, iResidue);
                    DecryptData(szTmp8Byte, keyN);
                    Array.Copy(szPlaintextInBytes, 0, szFPlaintextAnyLength, (iParts << 3), 8);
                }
            }
        }

        /// <summary>
        /// Byte 到 Bits 的转换
        /// </summary>
        /// <param name="srcBytes">待变换数据</param>
        /// <param name="dstBits">处理后结果存放缓冲区</param>
        /// <param name="sizeBits">Bits缓冲区大小</param>
        /// <remarks></remarks>
        private void Bytes2Bits(byte[] srcBytes, byte[] dstBits, Int32 sizeBits)
        {
            Int32 iData = 0;
            for (Int32 i = 0; i <= sizeBits - 1; i++)
            {
                iData = srcBytes[i >> 3];
                iData = iData << (i & 7);
                iData = iData & 128;
                iData = iData >> 7;
                dstBits[i] = (byte)iData;
            }
        }

        /// <summary>
        /// Bits到Byte的转换,
        /// </summary>
        /// <param name="dstBytes">存储转换后的Byte数组</param>
        /// <param name="srcBits">准备转换为byte的bit数组</param>
        /// <param name="sizeBits">Bits缓冲区大小</param>
        /// <remarks></remarks>
        private void Bits2Bytes(byte[] dstBytes, byte[] srcBits, Int32 sizeBits)
        {
            int i;
            // 初始化数组
            Array.Clear(dstBytes, 0, dstBytes.Length);
            // 开始抓换
            for (i = 0; i <= sizeBits - 1; i++)
                dstBytes[i >> 3] =(byte)( dstBytes[i >> 3] | (srcBits[i] << (7 - (i & 7))));
        }


        /// <summary>
        /// Int到Bits的转换,取低4bit
        /// </summary>
        /// <param name="src">待变换数值</param>
        /// <param name="dstBits">处理后结果存放缓冲区</param>
        /// <remarks></remarks>
        public void Int2Bits(byte src, byte[] dstBits)
        {
            Int32 iData;
            for (Int32 i = 0; i <= 4 - 1; i++)
            {
                iData = (src << i);
                iData = iData & 8;
                iData = iData >> 3;
                dstBits[i] =(byte) iData;
            }
        }

        

        


        /// <summary>
        /// 返回加密后的数据十六进制形式
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetCiphertextInHex()
        {
            return StringUtil.ByteToHex(GetCiphertextAnyLength());
        }


        /// <summary>
        /// 返回加密后的数据 -- 8字节
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte[] GetCiphertextInBytes()
        {
            byte[] bData;
            bData = new byte[8];
            Array.Copy(szCiphertextInBytes, bData, 8);

            return bData;
        }


        /// <summary>
        /// 返回加密后的所有数据
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte[] GetCiphertextAnyLength()
        {
            byte[] bData;
            bData = new byte[szFCiphertextAnyLengthMax - 1 + 1];
            Array.Copy(szFCiphertextAnyLength, bData, szFCiphertextAnyLengthMax);
            return bData;
        }

        /// <summary>
        /// 返回解密后的数据,明文字符串
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetPlaintext()
        {
            return System.Text.Encoding.Default.GetString(GetPlaintextAnyLength());
        }

        /// <summary>
        /// 返回解密后的数据 -- 8字节
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte[] GetPlaintextInBytes()
        {
            byte[] bData;
            bData = new byte[8];
            Array.Copy(szPlaintextInBytes, bData, 8);
            return bData;
        }

        /// <summary>
        /// 返回解密后的数据--十六进制形式
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetPlaintextInHex()
        {
            return StringUtil.ByteToHex(GetPlaintextAnyLength());
        }

        /// <summary>
        /// 返回明文数据,自动省略末尾的 byte(0)
        /// </summary>
        public byte[] GetPlaintextAnyLength()
        {
            Int32 lPlainLength;  // 明文长度
            bool bExitFor=false;
            lPlainLength = szFPlaintextAnyLengthMax;
            // 检查明文的借止符
            for (Int32 i = 0; i <= szFPlaintextAnyLengthMax - 1; i++)
            {
                if ((szFPlaintextAnyLength[i] == 0))
                {
                    bExitFor = true;
                    // 检测到 \0 ，继续检查是否结束
                    for (Int32 j = i + 1; j <= szFPlaintextAnyLengthMax - 1; j++)
                    {
                        if (!(szFPlaintextAnyLength[j] == 0))
                        {
                            bExitFor = false;
                            break;
                        }
                    }
                }

                if ((bExitFor))
                {
                    lPlainLength = i;
                    break;
                }
            }

            byte[] bData;
            bData = new byte[lPlainLength - 1 + 1];
            Array.Copy(szFPlaintextAnyLength, bData, lPlainLength);

            return bData;
        }

        /// <summary>
        /// 返回所有解密后的数据
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte[] GetPlaintextAll()
        {
            return (byte[])szFPlaintextAnyLength.Clone();
        }


        /// <summary>
        /// 初始化加密密钥
        /// </summary>
        /// <param name="bKeys">8字节数组</param>
        /// <param name="keyN">密钥的索引号0-1</param>
        /// <remarks>函数将调用private CreateSubKey将结果存于char SubKeys(keyN)(16)(48)</remarks>
        public void InitializeKey(byte[] bKeys, Int32 keyN)
        {
            // convert 8 char-bytes key to 64 binary-bits
            byte[] sz_64key = new byte[64];
            var oldBKeys = bKeys;
            bKeys = new byte[8];

            // 检查键值是否够8字节，不够8字节补零
            if (oldBKeys != null)
                Array.Copy(oldBKeys, bKeys, Math.Min(8, oldBKeys.Length));

            Bytes2Bits(bKeys, sz_64key, 64);
            // PC 1
            byte[] sz_56key = new byte[56];
            for (byte k = 0; k <= 56 - 1; k++)
                sz_56key[k] = sz_64key[PC1_Table[k] - 1];
            CreateSubKey(sz_56key, keyN);
        }

        /// <summary>
        /// 生成子密钥
        /// </summary>
        /// <param name="sz_56key">经过PC1变换的56位数组</param>
        /// <param name="keyN">密钥编号0-1</param>
        /// <remarks>将保存于char szSubKeys(16)(48)</remarks>
        private void CreateSubKey(byte[] sz_56key, Int32 keyN)
        {
            byte[] szTmpL, szTmpR;
            byte[] szCi, szDi;
            byte[] szTmp56 = new byte[56];
            szCi = new byte[28];
            szDi = new byte[28];
            szTmpL = new byte[28];
            szTmpR = new byte[28];

            Array.Copy(sz_56key, 0, szTmpL, 0, 28);
            Array.Copy(sz_56key, 28, szTmpR, 0, 28);

            for (Int32 i = 0; i <= 16 - 1; i++)
            {
                // shift to left
                // Left 28 bits
                Array.Copy(szTmpL, Shift_Table[i], szCi, 0, 28 - Shift_Table[i]);
                Array.Copy(szTmpL, 0, szCi, 28 - Shift_Table[i], Shift_Table[i]);

                // Right 28 bits
                Array.Copy(szTmpR, Shift_Table[i], szDi, 0, 28 - Shift_Table[i]);
                Array.Copy(szTmpR, 0, szDi, 28 - Shift_Table[i], Shift_Table[i]);

                // permuted choice 48 bits key
                Array.Copy(szCi, 0, szTmp56, 0, 28);
                Array.Copy(szDi, 0, szTmp56, 28, 28);

                for (Int32 j = 0; j <= 48 - 1; j++)

                    szSubKeys[keyN, i, j] = szTmp56[PC2_Table[j] - 1];
                // Evaluate new szTmpL and szTmpR
                Array.Copy(szCi, 0, szTmpL, 0, 28);
                Array.Copy(szDi, 0, szTmpR, 0, 28);
            }
        }


        /// <summary>
        /// 加密8字节数组
        /// </summary>
        /// <param name="bDatas">8字节数组</param>
        /// <param name="keyN">Key的序号0-1</param>
        /// <remarks>函数将加密后结果存放于private szCiphertext(16) 用户通过属性Ciphertext得到</remarks>
        public void EncryptData(byte[] bDatas, Int32 keyN)
        {
            byte[] bBits = new byte[64];
            byte[] sz_IP = new byte[64];
            byte[] sz_Li = new byte[32], sz_Ri = new byte[32];
            byte[] sz_Final64 = new byte[64];

            // 将8字节数据转换为64bit
            Bytes2Bits(bDatas, bBits, 64);
            // IP
            InitialPermuteData(bBits, sz_IP);

            Array.Copy(sz_IP, 0, sz_Li, 0, 32);
            Array.Copy(sz_IP, 32, sz_Ri, 0, 32);

            for (Int32 i = 0; i <= 16 - 1; i++)
                FunctionF(sz_Li, sz_Ri, i, keyN);
            // so D=LR

            Array.Copy(sz_Ri, 0, sz_Final64, 0, 32);
            Array.Copy(sz_Li, 0, sz_Final64, 32, 32);

            // ~IP
            for (Int32 j = 0; j <= 64 - 1; j++)
                szCiphertextRaw[j] = sz_Final64[IPR_Table[j] - 1];
            Bits2Bytes(szCiphertextInBytes, szCiphertextRaw, 64);
        }

        /// <summary>
        /// 解密8字节数组
        /// </summary>
        /// <param name="bDatas">8字节数组</param>
        /// <param name="keyN">Key的序号0-1</param>
        /// <remarks>函数将解密候结果存放于private szPlaintext(8) 用户通过属性Plaintext得到</remarks>
        public void DecryptData(byte[] bDatas, Int32 keyN)
        {
            byte[] bBits = new byte[64];
            byte[] sz_IP = new byte[64];
            byte[] sz_Li = new byte[32], sz_Ri = new byte[32];
            byte[] sz_Final64 = new byte[64];

            Bytes2Bits(bDatas, bBits, 64);
            // IP --- return is sz_IP
            InitialPermuteData(bBits, sz_IP);
            // divide the 64 bits data to two parts
            Array.Copy(sz_IP, 0, sz_Ri, 0, 32); // exchange L to R
            Array.Copy(sz_IP, 32, sz_Li, 0, 32); // exchange R to L
                                                 // 16 rounds F and xor and exchange
            for (Int32 i = 0; i <= 16 - 1; i++)

                FunctionF(sz_Ri, sz_Li, 15 - i, keyN);

            Array.Copy(sz_Li, 0, sz_Final64, 0, 32);
            Array.Copy(sz_Ri, 0, sz_Final64, 32, 32);

            // ~IP
            for (Int32 j = 0; j <= 64 - 1; j++)
                szPlaintextRaw[j] = sz_Final64[IPR_Table[j] - 1];
            Bits2Bytes(szPlaintextInBytes, szPlaintextRaw, 64);
        }

        /// <summary>
        /// IP变换
        /// </summary>
        /// <param name="src">待处理数组</param>
        /// <param name="dst">处理后结果</param>
        /// <remarks>函数改变第二个参数的内容</remarks>
        private void InitialPermuteData(byte[] src, byte[] dst)
        {
            // IP
            for (Int32 i = 0; i <= 64 - 1; i++)
                dst[i] = src[IP_Table[i] - 1];
        }

        /// <summary>
        /// DES中的F函数
        /// </summary>
        /// <param name="sz_Li">左32位</param>
        /// <param name="sz_Ri">右32位</param>
        /// <param name="iKey">key序号(0-15)</param>
        /// <param name="keyN">密钥序号0-1</param>
        /// <remarks>均在变换左右32位</remarks>
        private void FunctionF(byte[] sz_Li, byte[] sz_Ri, Int32 iKey, Int32 keyN)
        {
            byte[] sz_48R = new byte[48];
            byte[] sz_xor48 = new byte[48];
            byte[] sz_P32 = new byte[32];
            byte[] sz_Rii = new byte[32];
            byte[] sz_Key = new byte[48];
            byte[] s_Compress32 = new byte[32];

            // byte[2][16][48]    
            for (Int32 i = 0; i <= szSubKeys.GetLength(2) - 1; i++)
                sz_Key[i] = szSubKeys[keyN, iKey, i];

            ExpansionR(sz_Ri, sz_48R);
            XORArr(sz_48R, sz_Key, 48, sz_xor48);

            CompressFuncS(sz_xor48, s_Compress32);
            PermutationP(s_Compress32, sz_P32);
            XORArr(sz_P32, sz_Li, 32, sz_Rii);

            Array.Copy(sz_Ri, 0, sz_Li, 0, 32);
            Array.Copy(sz_Rii, 0, sz_Ri, 0, 32);
        }


        /// <summary>
        /// 将右32位进行扩展位48位
        /// </summary>
        /// <param name="src">原32位数组</param>
        /// <param name="dst">扩展后48位数组</param>
        /// <remarks>函数改变第二个参数的内容</remarks>
        private void ExpansionR(byte[] src, byte[] dst)
        {
            for (Int32 i = 0; i <= 48 - 1; i++)
                dst[i] = src[E_Table[i] - 1];
        }

        /// <summary>
        /// 异或函数,函数改变第四个参数的内容
        /// </summary>
        /// <param name="szParam1">待异或的数组1</param>
        /// <param name="szParam2">待异或的数组2</param>
        /// <param name="uiParamLength">数据长度</param>
        /// <param name="szReturnValueBuffer">结果返回数组</param>
        /// <remarks>函数改变第四个参数的内容</remarks>
        private void XORArr(byte[] szParam1, byte[] szParam2, Int32 uiParamLength, byte[] szReturnValueBuffer)
        {
            Int32 iData;
            for (Int32 i = 0; i <= uiParamLength - 1; i++)
            {
                iData = szParam1[i] ^ szParam2[i];
                szReturnValueBuffer[i] = (byte)iData;
            }
        }


        /// <summary>
        /// S-BOX , 数据压缩, 返回32位二进制数组
        /// </summary>
        /// <param name="src48">48位二进制数组</param>
        /// <param name="dst32">压缩后的32位二进制数组</param>
        /// <remarks></remarks>
        private void CompressFuncS(byte[] src48, byte[] dst32)
        {
            byte[,] bTemp = new byte[8, 6];
            byte[] dstBits = new byte[4];
            for (Int32 i = 0; i <= 8 - 1; i++)
            {
                for (Int32 j = 0; j <= 6 - 1; j++)
                    bTemp[i, j] = src48[(i * 6) + j];

                int iX = (bTemp[i, 0] * 2) + bTemp[i, 5];
                int iY = 0;
                for (Int32 j = 1; j <= 5 - 1; j++)
                    iY += bTemp[i, j] << (4 - j);
                Int2Bits(S_Box[i, iX, iY], dstBits);

                Array.Copy(dstBits, 0, dst32, i * 4, 4);
            }
        }


        /// <summary>
        /// IP逆变换,函数改变第二个参数的内容
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <remarks></remarks>
        private void PermutationP(byte[] src, byte[] dst)
        {
            for (Int32 i = 0; i <= 32 - 1; i++)
                dst[i] = src[P_Table[i] - 1];
        }

        /// <summary>
        ///将一个加密文件，解密后返回文件内容
        ///</summary>
        ///<param name="sFile"></param>
        ///<param name="sKey"></param>
        ///<returns></returns>
        public string GetDecryptFile(string sFile, string sKey)
        {
            if (!System.IO.File.Exists(sFile))
                return string.Empty;

            // 加载类型文件
            var bFileData = System.IO.File.ReadAllBytes(sFile);

            // 初始化解密器
            InitializeKey(gEncoding.GetBytes(sKey), 0);

            // 开始解密
            var bLen = bFileData.Copy(0, 4);
            var iFileLen = bLen.ToInt32Rev();
            bFileData = bFileData.Copy(4, bFileData.Count() - 4);

            DecryptAnyLength(bFileData, bFileData.Count(), 0);
            // 获取明文文件长度
            bFileData = GetPlaintextAll();
            bFileData = bFileData.Copy(0, (int)iFileLen);

            return gEncoding.GetString(bFileData);
        }


        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="sTest"></param>
        /// <param name="sKey1"></param>
        /// <param name="sKey2"></param>
        /// <returns></returns>
        public string EncryptText3Des(string sTest, string sKey1, string sKey2)
        {
            // 对长度在 8192(8K) 个字符以内的数据进行3DES加密
            if (string.IsNullOrEmpty(sTest))
                return string.Empty;

            byte[] bBytes;

            bBytes = gEncoding.GetBytes(sKey1);
            InitializeKey(bBytes, 0);

            bBytes = gEncoding.GetBytes(sKey2);
            InitializeKey(bBytes, 1);

            // 加密
            bBytes = gEncoding.GetBytes(sTest);
            // 用Key1 进行第一次加密
            EncryptAnyLength(bBytes, bBytes.Length, 0);
            // 用Key2 解密一次
            bBytes = GetCiphertextAnyLength();
            DecryptAnyLength(bBytes, bBytes.Length, 1);
            // 用Key1 进行第二次加密
            bBytes = GetPlaintextAnyLength();
            EncryptAnyLength(bBytes, bBytes.Length, 0);

            // 获取加密后的十六进制编码
            return GetCiphertextAnyLength().ToHex();
        }


        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="sTest"></param>
        /// <param name="sKey1"></param>
        /// <param name="sKey2"></param>
        /// <returns></returns>
        public string DecryptText3Des(string sTest, string sKey1, string sKey2)
        {
            // 对长度在 8192(8K) 个字符以内的数据进行3DES加密
            if (string.IsNullOrEmpty(sTest))
                return string.Empty;

            byte[] bBytes;

            bBytes = gEncoding.GetBytes(sKey1);
            InitializeKey(bBytes, 0);

            bBytes = gEncoding.GetBytes(sKey2);
            InitializeKey(bBytes, 1);

            // 转换十六进制字符串为字节数组
            bBytes = sTest.HexToByte();

            // 解密
            // 用Key1 进行第一次解密
            DecryptAnyLength(bBytes, bBytes.Length, 0);
            // 用Key2 加密一次
            bBytes = GetPlaintextAnyLength();
            EncryptAnyLength(bBytes, bBytes.Length, 1);
            // 用Key1 进行第二次解密
            bBytes = GetCiphertextAnyLength();
            DecryptAnyLength(bBytes, bBytes.Length, 0);

            // 获取解密后的数据
            bBytes = GetPlaintextAnyLength();
            // 转换为unicode字符
            return gEncoding.GetString(bBytes);
        }
    }

}
