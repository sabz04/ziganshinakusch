
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
        public static List<Good> recentlyDeleted = new List<Good>();
        public static DbModelContainer GetContext()
        {
            using(var db = new DbModelContainer())
            {
                return db;
            }
        }
        public static bool RegMethod(string login, string pass)
        {
            using (var db = new DbModelContainer())
            {

                var user =
                    db.Users.FirstOrDefault(x => x.Login == login);
                if (user != null)
                {
                    MessageBox.Show("Пользователь с таким именем уже зарегистрирован.");
                    return false;
                }
                if (login == "" || pass=="")
                {
                    MessageBox.Show("Не пытайся зарегистрировать пустого пользователя.");
                    return false;
                }
                if (login == pass || login.Contains(pass) || pass.Contains(login))
                {
                    MessageBox.Show("В целях безопасности, логин и пароль не должны быть равны и иметь схожести.");
                    return false;
                }
                if (login.Count() < 4 || pass.Count()< 4)
                {
                    MessageBox.Show("Логин и пароль должны содержать больше 4 символов.");
                    return false;
                }
                db.Users.Add(new User
                {
                    Login = login,
                    Password = pass,
                    Bucket = new Bucket() { Count = 0, TotalPrice = 0 },
                    Role = "user"

                });

                db.SaveChanges();
                MessageBox.Show("Регистрация успешна!");
                return true;
            }
        }
        public static User LogMethod(string login, string pass)
        {
            using (var db = new DbModelContainer())
            {
                var user =
                    db.Users.FirstOrDefault(x => x.Login == login && x.Password == pass);
                if (user == null)
                {
                    MessageBox.Show("Пройдите регистрацию.");
                    return null;
                }
                MessageBox.Show("Пользователь найден!");
                return user;
            }
        }
        public static byte[] GetByteFileFromExplorer()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Image Files|*.jpg; *.jpeg; *.png;"
            };
            openFileDialog.ShowDialog();
            string file = openFileDialog.FileName;
            if (file == "")
                return null;
            byte[] data = File.ReadAllBytes(file);
            return data;

        }
        public static bool IsFullAccess(User user)
        {
            if (user == null)
                return false;
            if (user.Email == "" || user.Password == "" || user.Phone == "" || user.FullName == "" || user.CardNumber == ""||
                user.Email == null || user.CardNumber == null || user.Phone == null || user.FullName == null) return false;
            return true;
        }
        public static bool AddNewGood(
            string name, string price, string info, string type, byte[] file)
        {
            try
            {
                if(name == "") {MessageBox.Show("Дайте товару хотя-бы название..."); return false; }
                using (var db = new DbModelContainer())
                {
                    db.GoodSet.Add(new Good
                    {
                        Name = name,
                        Price = int.Parse(price),
                        File = file,
                        Info = info,
                        Type = type
                    });
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            
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
        public static bool RemoveGoodFromBucket(User user,Good good)
        {
            if(user == null) return false;
            if(good == null) return false;
            using(var db = new DbModelContainer())
            {

                var user_act = 
                    db.Users.FirstOrDefault(x=>x.Id == user.Id);
                if (user_act == null)
                    return false;
                var good_act = 
                    db.GoodSet.FirstOrDefault(x=>x.Id == good.Id && x.BucketId == user_act.Bucket.Id);
                if(good_act == null) return false;
                if (good_act.Bucket != null) return false;
                db.GoodSet.Remove(good_act);
            }
            return true;

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
        public static User EditUser(int id,string login,
            string pass,string phone, string mail, string cardnum,string fullname )
        {
            if(id < 0){ return null; }
            using (var db = new DbModelContainer())
            {
                if (pass == "" || phone == "" || login == "" || pass == "" || mail == "" || cardnum == "" || fullname == "") {
                    MessageBox.Show("Не пытайтесь изменить на пустую строку.");
                    return null; 
                }
                var user =
                   db.Users.FirstOrDefault(u => u.Id == id);
                if (user == null) return null;
                user.Email =mail;
                user.Phone = phone;
                user.Password = pass;
                user.Login = login;
                user.CardNumber = cardnum;
                user.FullName = fullname;
                db.SaveChanges();
                return user;
            }
            
        }
        public static bool AddGoodToBucket(User cuser, Good cgood)
        {
            if(cuser == null) return false;
            if (cgood == null) return false;
            using (var db = new DbModelContainer()) {
                var user =
                    db.Users.FirstOrDefault(x=>x.Id == cuser.Id);
                if(user == null) return false;
                var good = 
                    db.GoodSet.FirstOrDefault(x=>x.Id == cgood.Id);
                if(good == null) return false;
                if (good.Bucket != null)
                {
                    MessageBox.Show("Этот товар кто-то уже занял!!");
                    return false;
                }
                    
                user.Bucket.Good.Add(good);
                db.SaveChanges();
                return true;
            }
        }
        public static byte[] CompressByImageAlg(int jpegQuality, byte[] data)
        {
            if(data==null) return null;
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
