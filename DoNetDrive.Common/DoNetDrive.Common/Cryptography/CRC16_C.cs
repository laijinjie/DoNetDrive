using System.Collections.Generic;

namespace DoNetDrive.Common.Cryptography
{
    public class CRC16_C
    {
        public static int CreateCRC16(byte[] bData)
        {
            int iCRC16 = 0;

            byte CRC16Lo, CRC16Hi, CL, CH, SaveHi, SaveLo;
            int i, Flag;
            int datalen = bData.Length - 1;
            byte bH80 = 0x80;

            CRC16Lo = 255; // &HFF
            CRC16Hi = 255; // &HFF
            CL = 1; // &H01
            CH = 160; // &HA0

            for (i = 0; i <= datalen; i++)
            {
                CRC16Lo =(byte)( CRC16Lo ^ bData[i]);
                for (Flag = 0; Flag <= 7; Flag++)
                {
                    SaveHi = CRC16Hi;
                    SaveLo = CRC16Lo;
                    CRC16Hi >>= 1;
                    CRC16Lo >>= 1;
                    if ((SaveHi & 1) == 1)
                        CRC16Lo =(byte)( CRC16Lo | bH80);
                    if ((SaveLo & 1) == 1)
                    {
                        CRC16Hi = (byte)(CRC16Hi ^ CH);
                        CRC16Lo = (byte)(CRC16Lo ^ CL);
                    }
                }
            }
            iCRC16 = CRC16Hi;
            iCRC16 = (iCRC16 << 8) | CRC16Lo;
            return iCRC16;
        }


        public static int CreateCRC16(List<byte> bData, int lBeginIndex, int lLen)
        {
            int iCRC16 = 0;

            byte CRC16Lo, CRC16Hi, CL, CH, SaveHi, SaveLo;
            int i, Flag;
            int datalen = (lBeginIndex + lLen) - 1;
            byte bH80 = 0x80;

            CRC16Lo = 255; // &HFF
            CRC16Hi = 255; // &HFF
            CL = 1; // &H01
            CH = 160; // &HA0

            for (i = lBeginIndex; i <= datalen; i++)
            {
                CRC16Lo = (byte)(CRC16Lo ^ bData[i]);
                for (Flag = 0; Flag <= 7; Flag++)
                {
                    SaveHi = CRC16Hi;
                    SaveLo = CRC16Lo;
                    CRC16Hi >>= 1;
                    CRC16Lo >>= 1;
                    if ((SaveHi & 1) == 1)
                        CRC16Lo = (byte)(CRC16Lo | bH80);
                    if ((SaveLo & 1) == 1)
                    {
                        CRC16Hi = (byte)(CRC16Hi ^ CH);
                        CRC16Lo = (byte)(CRC16Lo ^ CL);
                    }
                }
            }
            iCRC16 = CRC16Hi;
            iCRC16 = (iCRC16 << 8) | CRC16Lo;
            return iCRC16;
        }


        public static int CreateCRC16(byte[] bData, int lBeginIndex, int lLen)
        {
            int iCRC16 = 0;

            byte CRC16Lo, CRC16Hi, CL, CH, SaveHi, SaveLo;
            int i, Flag;
            int datalen = (lBeginIndex + lLen) - 1;
            byte bH80 = 0x80;

            CRC16Lo = 255; // &HFF
            CRC16Hi = 255; // &HFF
            CL = 1; // &H01
            CH = 160; // &HA0

            for (i = lBeginIndex; i <= datalen; i++)
            {
                CRC16Lo = (byte)(CRC16Lo ^ bData[i]);
                for (Flag = 0; Flag <= 7; Flag++)
                {
                    SaveHi = CRC16Hi;
                    SaveLo = CRC16Lo;
                    CRC16Hi >>= 1;
                    CRC16Lo >>= 1;
                    if ((SaveHi & 1) == 1)
                        CRC16Lo = (byte)(CRC16Lo | bH80);
                    if ((SaveLo & 1) == 1)
                    {
                        CRC16Hi = (byte)(CRC16Hi ^ CH);
                        CRC16Lo = (byte)(CRC16Lo ^ CL);
                    }
                }
            }
            iCRC16 = CRC16Hi;
            iCRC16 = (iCRC16 << 8) | CRC16Lo;
            return iCRC16;
        }
    }

}
