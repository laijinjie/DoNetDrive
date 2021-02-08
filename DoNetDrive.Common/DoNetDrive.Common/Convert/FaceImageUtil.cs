using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNetDrive.Common
{
    public class FaceImageUtil
    {
        /// <summary>
        /// 文件最大尺寸
        /// </summary>
        private const int ImageSizeMax = 153600;
        /// <summary>
        /// 进行图片转换，图片像素不能超过 480*640，大小尺寸不能超过50K
        /// </summary>
        /// <param name="strFile"></param>
        /// <returns></returns>
        public static byte[] ConvertImage(byte[] bImage)
        {
            Image img = Image.FromStream(new System.IO.MemoryStream(bImage));
            float rate = 1;
            if (img.Width > 480 || img.Height > 640 || bImage.Length > ImageSizeMax)
            {
                float rate1, rate2;

                rate1 = (float)480 / (float)img.Width;
                rate2 = (float)640 / (float)img.Height;
                rate = rate1 > rate2 ? rate2 : rate1;
                //if (rate > 1) rate = 1;

            }
            else
            {
                img.Dispose();
                img = null;
                return bImage;
            }
            int iWidth = img.Width, iHeight = img.Height;
            iWidth = (int)(iWidth * rate);
            iHeight = (int)(iHeight * rate);
            byte[] newFile = null;
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            // 创建一个EncoderParameters对象.
            // 一个EncoderParameters对象有一个EncoderParameter数组对象
            EncoderParameters myEncoderParameters = new EncoderParameters(1);



            using (Bitmap bimg = new Bitmap(480, 640, PixelFormat.Format32bppArgb))
            {
                using (Graphics graphics = Graphics.FromImage(bimg))
                {
                    graphics.PageUnit = GraphicsUnit.Pixel;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.Clear(Color.White);
                    graphics.DrawImage(img, new Rectangle((480 - iWidth) / 2, (640 - iHeight) / 2, iWidth, iHeight));
                    graphics.Dispose();
                }

                //进行图片大小的测算
                long iQuality = 80;
                bool bSave = false;
                do
                {
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, iQuality);//这里用来设置保存时的图片质量
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bimg.Save(ms, jgpEncoder, myEncoderParameters);
                        myEncoderParameter.Dispose();
                        int iNewLen = (int)ms.Length;
                        if (iNewLen <= ImageSizeMax)
                        {
                            newFile = new byte[iNewLen];
                            ms.Position = 0;
                            ms.Read(newFile, 0, iNewLen);
                            bSave = true;
                        }
                        ms.Close();
                        ms.Dispose();

                        iQuality -= 5;
                    }
                } while (!bSave);

            }

            return newFile;

        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
