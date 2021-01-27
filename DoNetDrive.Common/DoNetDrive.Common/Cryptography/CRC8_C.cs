using System.Collections.Generic;

namespace DoNetDrive.Common.Cryptography
{
    

    public class CRC8_C
    {
        public static int CreateCRC8(byte[] bData)
        {
            int crc = 0;
            int i = 0;
            int len = bData.Length;
            int iIndex=0;

            while ((len > iIndex))
            {
                // crc^=*ptr++;
                crc = crc ^ bData[iIndex];
                for (i = 1; i <= 8; i++)
                {
                    if (((crc & 1) == 1))
                        crc = (crc >> 1) ^ 0x8;
                    else
                        crc = crc >> 1;
                }

                iIndex += 1;
            }
            return crc;
        }


        public static int CreateCRC8(List<byte> bData, int lBeginIndex, int lLen)
        {
            int crc = 0;
            int i = 0;
            int len = lBeginIndex + lLen;
            int iIndex = lBeginIndex;

            while ((len > iIndex))
            {
                // crc^=*ptr++;
                crc = crc ^ bData[iIndex];
                for (i = 1; i <= 8; i++)
                {
                    if (((crc & 1) == 1))
                        crc = (crc >> 1) ^ 0x8;
                    else
                        crc = crc >> 1;
                }

                iIndex += 1;
            }
            return crc;
        }

        public static int CreateCRC8(byte[] bData, int lBeginIndex, int lLen)
        {
            int crc = 0;
            int i = 0;
            int len = lBeginIndex + lLen;
            int iIndex = lBeginIndex;

            while ((len > iIndex))
            {
                // crc^=*ptr++;
                crc = crc ^ bData[iIndex];
                for (i = 1; i <= 8; i++)
                {
                    if (((crc & 1) == 1))
                        crc = (crc >> 1) ^ 0x8;
                    else
                        crc = crc >> 1;
                }

                iIndex += 1;
            }
            return crc;
        }
    }

}
