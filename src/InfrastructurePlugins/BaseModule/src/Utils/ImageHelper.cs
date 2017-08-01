using System;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;

namespace InfrastructurePlugins.BaseModule.Utils
{
    public static class ImageHelper
    {
        public static Bitmap CloneBitmap(Image source)
        {
            if (source == null)
            {
                return null;
            }
            int width = source.Width;
            int height = source.Height;
            Bitmap image = new Bitmap(width, height);
            image.SetResolution(source.HorizontalResolution, source.VerticalResolution);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.DrawImageUnscaled(source, 0, 0);
            }
            return image;
        }

        public static Bitmap GetTransparentBitmap(Image source, float transparency)
        {
            if (source == null)
            {
                return null;
            }
            ColorMatrix newColorMatrix = new ColorMatrix
            {
                Matrix33 = 1f - transparency
            };
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            int width = source.Width;
            int height = source.Height;
            Bitmap image = new Bitmap(width, height);
            image.SetResolution(source.HorizontalResolution, source.VerticalResolution);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.Clear(Color.Transparent);
                graphics.DrawImage(source, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel, imageAttr);
            }
            return image;
        }

        public static Image Load(Stream stream)
        {
            if ((stream != null) && (stream.Length > 0L))
            {
                byte[] buffer = new byte[4];
                long position = stream.Position;
                stream.Read(buffer, 0, 4);
                stream.Position = position;
                if (((buffer[0] == 0xd7) && (buffer[1] == 0xcd)) && ((buffer[2] == 0xc6) && (buffer[3] == 0x9a)))
                {
                    return new Metafile(stream);
                }
                byte[] buffer2 = new byte[0x2c];
                position = stream.Position;
                stream.Read(buffer2, 0, 0x2c);
                stream.Position = position;
                if (((buffer2[40] == 0x20) && (buffer2[0x29] == 0x45)) && ((buffer2[0x2a] == 0x4d) && (buffer2[0x2b] == 70)))
                {
                    return new Metafile(stream);
                }
                try
                {
                    using (Bitmap bitmap = new Bitmap(stream))
                    {
                        return CloneBitmap(bitmap);
                    }
                }
                catch
                {
                    Bitmap image = new Bitmap(10, 10);
                    using (Graphics graphics = Graphics.FromImage(image))
                    {
                        graphics.DrawLine(Pens.Red, 0, 0, 10, 10);
                        graphics.DrawLine(Pens.Red, 0, 10, 10, 0);
                    }
                    return image;
                }
            }
            return null;
        }

        public static Image Load(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    return Load(stream);
                }
            }
            return null;
        }

        public static Image LoadURL(string url)
        {
            //if (!string.IsNullOrEmpty(url))
            //{
            //    using (WebClient client = new WebClient())
            //    {
            //        using (MemoryStream stream = new MemoryStream(client.DownloadData(url)))
            //        {
            //            return Load(stream);
            //        }
            //    }
            //}
            return null;
        }

        public static void Save(Image image, Stream stream)
        {
            Save(image, stream, ImageFormat.Png);
        }

        public static void Save(Image image, Stream stream, ImageFormat format)
        {
            if (image != null)
            {
                if (image is Bitmap)
                {
                    image.Save(stream, format);
                }
                else if (image is Metafile)
                {
                    Metafile metafile = null;
                    using (Bitmap bitmap = new Bitmap(1, 1))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            IntPtr hdc = graphics.GetHdc();
                            metafile = new Metafile(stream, hdc);
                            graphics.ReleaseHdc(hdc);
                        }
                    }
                    using (Graphics graphics2 = Graphics.FromImage(metafile))
                    {
                        graphics2.DrawImage(image, 0, 0);
                    }
                }
            }
        }

        public static void Save(Image image, string fileName, ImageFormat format)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                Save(image, stream, format);
            }
        }
    }
}
