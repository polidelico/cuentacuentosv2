using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cuentos.Lib.Utils
{
    public class Uploader
    {
        public static string StorageRootPath = "~/Content/dynamic";
        public static string StorageRootPath_Server = "/Content/dynamic";

        public static string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }

        public enum ImageQuality
        {
            LOW, MED, HIGH
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                using (ImageAttributes wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage(image, new Rectangle(0, 0, newImage.Width, newImage.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return newImage;
        }

        public static void ScaleAndSaveImage(Image image, int maxWidth, int maxHeight, ImageQuality quality, string path)
        {
            SaveToDisk(ScaleImage(image, maxWidth, maxHeight), quality, path);
        }

        public static void SaveToDisk(Image image, ImageQuality quality, string path)
        {
            Int64 q = 100L;
            if (quality == ImageQuality.LOW)
            {
                q = 50L;
            }
            else if (quality == ImageQuality.MED)
            {
                q = 75L;
            }

            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, q);
            myEncoderParameters.Param[0] = myEncoderParameter;
            image.Save(path, jgpEncoder, myEncoderParameters);
        }


        public static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (Image)(bmpCrop);
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

        public static UploadFilesResult UploadFile(Controller controller, HttpPostedFileBase file, int assetId, bool emptyDir)
        {
            var storageRoot = Path.Combine(controller.Server.MapPath(StorageRootPath));
            var path = Path.Combine(storageRoot, assetId.ToString());
            var fullPath = Path.Combine(path, Path.GetFileName(file.FileName));

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            else
            {
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(path);

                if (emptyDir)
                    directory.Empty();
            }

            file.SaveAs(fullPath);

            return new UploadFilesResult()
            {
                name = file.FileName,
                size = file.ContentLength,
                type = file.ContentType,
                delete_url = "/assets/delete/" + assetId,
                thumbnail_url = @"data:image/png;base64," + Uploader.EncodeFile(fullPath),
                delete_type = "POST",
                url = controller.Url.Content("~/Content/dynamic/" + file.FileName),
                id = assetId
            };
        }

        public static void RemoveFile(Controller controller, string assetId, string filename)
        {
            var storageRoot = Path.Combine(controller.Server.MapPath(StorageRootPath));
            var path = Path.Combine(storageRoot, assetId);
            var fullPath = Path.Combine(path, Path.GetFileName(filename));

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

        }

        public static UploadFilesResult UploadAndScaleFile(Controller controller, HttpPostedFileBase file, int assetId, Size size)
        {
            return UploadAndScaleFile(controller, file, assetId, size.Width, size.Height);
        }

        public static UploadFilesResult UploadAndScaleFile(Controller controller, HttpPostedFileBase file, int assetId, int width, int height)
        {
            var storageRoot = Path.Combine(controller.Server.MapPath(StorageRootPath));
            var path = Path.Combine(storageRoot, assetId.ToString());
            var fullPath = Path.Combine(path, Path.GetFileName(file.FileName));

            System.Drawing.Image scaledImage = System.Drawing.Image.FromStream(file.InputStream);
            scaledImage = ScaleImage(scaledImage, width, height);


            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            else
            {
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(path);
                directory.Empty();
            }

            scaledImage.Save(fullPath);

            var fileLength = new FileInfo(fullPath).Length;

            return new UploadFilesResult()
            {
                name = file.FileName,
                size = int.Parse(fileLength.ToString()),
                type = file.ContentType,
                delete_url = "/assets/delete/" + assetId,
                thumbnail_url = @"data:image/png;base64," + Uploader.EncodeFile(fullPath),
                delete_type = "POST",
                url = controller.Url.Content("~/Content/dynamic/" + file.FileName),
                id = assetId
            };
        }
    }

    public static class IOFunctions
    {
        public static void Empty(this System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }
    }


    public class UploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string delete_url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_type { get; set; }
        public int id { get; set; }
    }
}