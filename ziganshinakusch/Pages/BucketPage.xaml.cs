using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ziganshinakusch.Model;
using ziganshinakusch.Windows;

namespace ziganshinakusch.Pages
{
    /// <summary>
    /// Логика взаимодействия для BucketPage.xaml
    /// </summary>
    public partial class BucketPage : Page
    {
        Style tbstyle = Application.Current.FindResource("tbStandart3") as Style;
        Style btnstyle = Application.Current.FindResource("btnStyle1") as Style;
        public BucketPage()
        {
            InitializeComponent();
            LoadGoods();
        }

        private void LoadGoods()
        {
            goodsPanel.Children.Clear();
            using (var db = new DbModelContainer())
            {
                var user =
                    db.Users.FirstOrDefault(x => x.Id == HomeWindow.currentUser.Id);
                var img_like = new Image();
                var goods =
                    user.Bucket.Good;
                foreach (var good in goods)
                {
                    img_like = new Image { Source = Opers.LoadImage(File.ReadAllBytes("./Images/delete.png")) };
                    var stck = new StackPanel() { Margin = new Thickness(10) };
                    stck.Children.Add(new Image { Height = 200, Source = Opers.LoadImage(File.ReadAllBytes("./Images/women.jpeg")) });
                    stck.Children.Add(new TextBlock
                    {
                        FontSize = 24,
                        Style = tbstyle,
                        TextAlignment = TextAlignment.Left,
                        FontWeight = FontWeights.Bold,
                        Text = good.Price.ToString() + "р"
                    });
                    stck.Children.Add(new TextBlock { TextAlignment = TextAlignment.Left, Style = tbstyle, Text = good.Name });
                    stck.Children.Add(new TextBlock { FontSize = 14, FontWeight = FontWeights.Thin, TextAlignment = TextAlignment.Left, Style = tbstyle, Text = good.Type });
                    var img_info = new Image { Source = Opers.LoadImage(File.ReadAllBytes("./Images/info.png")) };
                    var wrp = new WrapPanel() { HorizontalAlignment = HorizontalAlignment.Center };

                    var deleted_btn = new Button { Height = 30, Style = btnstyle, Content = img_like, Name = "button" + good.Id };
                    var info_btn = new Button { Height = 30, Style = btnstyle, Content = img_info, Name = "but" + good.Id };
                    deleted_btn.Click += Deleted_btn_Click;
                    info_btn.Click += Info_btn_Click;

                    wrp.Children.Add(deleted_btn);
                    wrp.Children.Add(info_btn);
                    stck.Children.Add(wrp);
                    goodsPanel.Children.Add(stck);


                }
                totalPriceTb.Text = $"Сумма: {user.Bucket.TotalPrice}";
                countTB.Text = $"Кол-во товаров:{user.Bucket.Count}";
            }

        }

        private void Info_btn_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DbModelContainer())
            {
                var id =
                    int.Parse((sender as Button).Name.Substring(3, (sender as Button).Name.Length - 3));
                var good =
                    db.GoodSet.FirstOrDefault(x => x.Id == id);
                MessageBox.Show($"~Подробная информация: \n{good.Info}");
            }
        }

        private void Deleted_btn_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DbModelContainer())
            {
                var id =
                    int.Parse((sender as Button).Name.Substring(6, (sender as Button).Name.Length - 6));
                var good =
                    db.GoodSet.FirstOrDefault(x => x.Id == id);
                var user =
                    db.Users.FirstOrDefault(x => x.Id == HomeWindow.currentUser.Id);
                if (user.Bucket == null)
                    user.Bucket = new Bucket();


                user.Bucket.Good.Remove(good);
                var img_good = (Image)(sender as Button).Content;

                good.Bucket = null;
                db.SaveChanges();
                Opers.UpdateBucket(user);
                LoadGoods();
            }
            HomeWindow.home.CheckUser();
        }

        private void backBTN_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow.home
                .homeFrame.Navigate(new GoodsPage());
        }

        private void payBTN_Click(object sender, RoutedEventArgs e)
        {
            using(var db = new DbModelContainer())
            {
                
                var user =
                    db.Users.FirstOrDefault(x=>x.Id == HomeWindow.currentUser.Id);
                if (!Opers.IsFullAccess(user))
                {
                    MessageBox.Show("Пожалуйста, заполните все необходомимые данные в личном кабинете.");
                    return;
                }
                    var items = "";
                    user.Bucket.Good.ToList().ForEach(x=>items+=$"\n{x.Name}");
                if(user.Bucket.Good.Count < 1) { MessageBox.Show("Не пытайтесь создать пустой заказ."); return; }
                PayWindow pay = new PayWindow(user);
                pay.Show();
            }
            HomeWindow.home.CheckUser();


        }
    }
}
