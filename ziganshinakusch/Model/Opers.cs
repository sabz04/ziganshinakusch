
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ziganshinakusch.Model
{
    public class Opers
    {
        public static DbModelContainer GetContext()
        {
            using(var db = new DbModelContainer())
            {
                return db;
            }
        }
        public static bool IsFullAccess(User user)
        {
            if (user.Email == "" || user.Password == "" || user.Phone == "" || user.FullName == "" || user.CardNumber == ""||
                user.Email == null || user.CardNumber == null || user.Phone == null || user.FullName == null) return false;
            return true;
        }
        public static bool IsHasGoodInBucket(User user)
        {
            using (var db = new DbModelContainer())
            {
                var cuser =
                    db.Users.FirstOrDefault(x => x.Id == user.Id);
                if (cuser.Bucket.Good.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsHasOrder(User user)
        {
            using (var db = new DbModelContainer())
            {
                var cuser =
                    db.Users.FirstOrDefault(x => x.Id == user.Id);
                if (cuser.Bucket.Order.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public static void UpdateBucket(User user)
        {
            using(var db = new DbModelContainer())
            {
                var curUser=
                    db.Users.FirstOrDefault(u => u.Id == user.Id);
                if (curUser == null) return;
                curUser.Bucket.TotalPrice = curUser.Bucket.Good.ToList().Select(x => x.Price).Sum();
                curUser.Bucket.Count = curUser.Bucket.Good.Count();
                db.SaveChanges();
            }
        }
        public static byte[] CompressByImageAlg(int jpegQuality, byte[] data)
        {
            using (MemoryStream inputStream = new MemoryStream(data))
            {
                using (Image image = Image.FromStream(inputStream))
                {
                    ImageCodecInfo jpegEncoder = ImageCodecInfo.GetImageDecoders()
                        .First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, jpegQuality);
                    byte[] outputBytes = null;
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        image.Save(outputStream, jpegEncoder, encoderParameters);
                        return outputStream.ToArray();
                    }
                }
            }
        }
        public static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
