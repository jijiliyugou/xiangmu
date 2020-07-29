using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Yaeher.Common
{
    public class ThumNail
    {
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// string originalImagePath = Server.MapPath("ImgUpLoad/" + fileName);    //服务器端文件路径
        ///  string thumNailPath = Server.MapPath("ImgUpLoad/" + sFileName);//缩略图文件路径
        /// <param name="orginalImagePat">原图片地址</param>
        /// <param name="thumNailPath">缩略图地址</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="model">生成缩略的模式</param>
        public void MakeThumNail(string originalImagePath, string thumNailPath, int width, int height, string model)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int thumWidth = width;      //缩略图的宽度
            int thumHeight = height;    //缩略图的高度

            int x = 0;
            int y = 0;

            int originalWidth = originalImage.Width;    //原始图片的宽度
            int originalHeight = originalImage.Height;  //原始图片的高度

            switch (model)
            {
                case "HW":      //指定高宽缩放,可能变形
                    break;
                case "W":       //指定宽度,高度按照比例缩放
                    thumHeight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H":       //指定高度,宽度按照等比例缩放
                    thumWidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut":
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)thumWidth / (double)thumHeight)
                    {
                        originalHeight = originalImage.Height;
                        originalWidth = originalImage.Height * thumWidth / thumHeight;
                        y = 0;
                        x = (originalImage.Width - originalWidth) / 2;
                    }
                    else
                    {
                        originalWidth = originalImage.Width;
                        originalHeight = originalWidth * height / thumWidth;
                        x = 0;
                        y = (originalImage.Height - originalHeight) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(thumWidth, thumHeight);

            //新建一个画板
            System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量查值法
            //graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //设置高质量，低速度呈现平滑程度
            //graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            //清空画布并以透明背景色填充
            graphic.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            graphic.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, thumWidth, thumHeight), new System.Drawing.Rectangle(x, y, originalWidth, originalHeight), System.Drawing.GraphicsUnit.Pixel);

            try
            {
                bitmap.Save(thumNailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                graphic.Dispose();
            }

        }
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// string originalImagePath = Server.MapPath("ImgUpLoad/" + fileName);    //服务器端文件路径
        ///  string thumNailPath = Server.MapPath("ImgUpLoad/" + sFileName);//缩略图文件路径
        /// <param name="orginalImagePat">原图片地址</param>
        /// <param name="thumNailPath">缩略图地址</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="model">生成缩略的模式</param>
        public Stream MakeThumNail(Image originalImagePath, Stream thumNailPath, int width, int height, string model)
        {
            System.Drawing.Image originalImage = originalImagePath;

            int thumWidth = width;      //缩略图的宽度
            int thumHeight = height;    //缩略图的高度

            int x = 0;
            int y = 0;

            int originalWidth = originalImage.Width;    //原始图片的宽度
            int originalHeight = originalImage.Height;  //原始图片的高度

            switch (model)
            {
                case "HW":      //指定高宽缩放,可能变形
                    break;
                case "W":       //指定宽度,高度按照比例缩放
                    thumHeight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H":       //指定高度,宽度按照等比例缩放
                    thumWidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut":
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)thumWidth / (double)thumHeight)
                    {
                        originalHeight = originalImage.Height;
                        originalWidth = originalImage.Height * thumWidth / thumHeight;
                        y = 0;
                        x = (originalImage.Width - originalWidth) / 2;
                    }
                    else
                    {
                        originalWidth = originalImage.Width;
                        originalHeight = originalWidth * height / thumWidth;
                        x = 0;
                        y = (originalImage.Height - originalHeight) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(thumWidth, thumHeight);

            //新建一个画板
            System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量查值法
            //graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //设置高质量，低速度呈现平滑程度
            //graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            //清空画布并以透明背景色填充
            graphic.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            graphic.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, thumWidth, thumHeight), new System.Drawing.Rectangle(x, y, originalWidth, originalHeight), System.Drawing.GraphicsUnit.Pixel);

            try
            {
                bitmap.Save(thumNailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                return thumNailPath;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                graphic.Dispose();
            }

        }

        /// <summary>
        /// 高质量缩放图片
        /// </summary>
        /// <param name="OriginFilePath">源图的路径</param>
        /// <param name="TargetFilePath">存储缩略图的路径</param>
        /// <param name="WaterFilePath">水印图的路径</param>
        /// <param name="WithWaterImge">是否添加水印</param>
        /// <param name="DestWidth">缩放后图片宽度</param>
        /// <param name="DestHeight">缩放后图片高度</param>
        /// <returns>表明此次操作是否成功</returns>
        public bool MakeThumNail2(string OriginFilePath, string TargetFilePath, string WaterFilePath, bool WithWaterImge, int DestWidth, int DestHeight, string model)
        {
            Image OriginImage = Image.FromFile(OriginFilePath);

            System.Drawing.Imaging.ImageFormat thisFormat = OriginImage.RawFormat;
            //按比例缩放
            int sW = 0, sH = 0;
            int ImageWidth = OriginImage.Width;
            int ImageHeight = OriginImage.Height;

            if (ImageWidth > DestWidth || ImageHeight > DestHeight)
            {
                if ((ImageWidth * DestWidth) > (ImageHeight * DestHeight))
                {
                    sW = DestWidth;
                    sH = (DestHeight * ImageHeight) / ImageWidth;
                }
                else
                {
                    sH = DestHeight;
                    sW = (DestWidth * ImageWidth) / ImageHeight;
                }
            }
            else
            {
                sW = ImageWidth;
                sH = ImageHeight;
            }

            Bitmap bt = new Bitmap(DestWidth, DestHeight); //根据指定大小创建Bitmap实例
            using (Graphics g = Graphics.FromImage(bt))
            {
                g.Clear(Color.White);
                //设置画布的描绘质量
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(OriginImage, new Rectangle((DestWidth - sW) / 2, (DestHeight - sH) / 2, sW, sH));
                if (WithWaterImge)
                {
                    Image WaterImage = Image.FromFile(WaterFilePath);
                    g.DrawImage(WaterImage, new Rectangle((DestWidth - sW) / 2, (DestHeight - sH) / 2, sW, sH));
                }
                g.Dispose();
            }

            System.Drawing.Imaging.EncoderParameters EncoderParams = new System.Drawing.Imaging.EncoderParameters(); //取得内置的编码器
            long[] Quality = new long[1];
            Quality[0] = 100;
            System.Drawing.Imaging.EncoderParameter EncoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Quality);
            EncoderParams.Param[0] = EncoderParam;

            try
            {
                //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象
                System.Drawing.Imaging.ImageCodecInfo[] arrayICI = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
                System.Drawing.Imaging.ImageCodecInfo jpegICI = null;
                for (int i = 0; i < arrayICI.Length; i++)
                {
                    if (arrayICI[i].FormatDescription.Equals("JPEG"))
                    {
                        jpegICI = arrayICI[i]; //设置为JPEG编码方式       z
                        break;
                    }
                }

                if (jpegICI != null) //保存缩略图
                {
                    bt.Save(TargetFilePath, jpegICI, EncoderParams);
                }
                else
                {
                    bt.Save(TargetFilePath, thisFormat);
                }

                OriginImage.Dispose();
                return true;
            }
            catch (System.Runtime.InteropServices.ExternalException e1)  //GDI+发生一般错误
            {
                return false;
            }
            catch (Exception e2)
            {
                return false;
            }
        }


        /// <summary>
        /// 在图片上添加文字水印
        /// </summary>
        /// <param name="path">要添加水印的图片路径</param>
        /// <param name="syPath">生成的水印图片存放的位置</param>
        public void AddWaterWord(string path, string syPath)
        {
            string syWord = "http://www.hello36.cn";
            System.Drawing.Image image = System.Drawing.Image.FromFile(path);

            //新建一个画板
            System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(image);
            graphic.DrawImage(image, 0, 0, image.Width, image.Height);

            //设置字体
            System.Drawing.Font f = new System.Drawing.Font("Verdana", 60);

            //设置字体颜色
            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Green);

            graphic.DrawString(syWord, f, b, 35, 35);
            graphic.Dispose();

            //保存文字水印图片
            image.Save(syPath);
            image.Dispose();

        }

        /// <summary>
        /// 在图片上添加图片水印
        /// </summary>
        /// <param name="path">要打水印的图片路径</param>
        /// <param name="syPicPath">水印图片的路径</param>
        /// <param name="waterPicPath">生成后水印图片存放路径</param>
        public void AddWaterPic(string path, string syPicPath, string filename)
        {

            System.Drawing.Image image = System.Drawing.Image.FromFile(path);
            System.Drawing.Imaging.ImageFormat thisFormat = image.RawFormat;

            System.Drawing.Image waterImage = System.Drawing.Image.FromFile(syPicPath);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(waterImage, new System.Drawing.Rectangle(image.Width - waterImage.Width, image.Height - waterImage.Height, waterImage.Width, waterImage.Height), 0, 0, waterImage.Width, waterImage.Height, System.Drawing.GraphicsUnit.Pixel);
            g.Dispose();

            image.Save(filename, thisFormat);
            image.Dispose();
        }
    }
}
