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
using System.Windows.Shapes;
using ziganshinakusch.Model;
using ziganshinakusch.Pages;

namespace ziganshinakusch.Windows
{
    /// <summary>
    /// Логика взаимодействия для HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public static HomeWindow home;
        public static User currentUser;
        public HomeWindow(User User)
        {
            InitializeComponent();
            home = this;
            currentUser = User;
            homeFrame.Navigate(new GoodsPage());
            CheckUser();
            Refresh();
        }
        public void Refresh(bool flag = true)
        {
            typesCB.Items.Clear();
            using(var db = new DbModelContainer())
            {
                typesCB.Items.Add("Все товары");
                db.GoodSet.Select(x => x.Type).Distinct().ToList().ForEach(x => typesCB.Items.Add(x));
                if (typesCB.Items.Count > 0 && flag)
                    typesCB.SelectedIndex = 0;
            }
        }
        public void CheckUser()
        {
            using(var db = new DbModelContainer())
            {
                var current = 
                    db.Users.FirstOrDefault(x=>x.Id== currentUser.Id);
                var img = (Image)accountBTN.Content;
                var img_order = (Image)orderBTN.Content;
                var img_bucket = (Image)bucketPageBTN.Content;
                if (!Opers.IsFullAccess(current))
                {

                    img.Source = Opers.LoadImage(File.ReadAllBytes("./Images/accounterror.png"));
                }
                else
                {
                    img.Source = Opers.LoadImage(File.ReadAllBytes("./Images/account.png"));
                }
                if (Opers.IsHasGoodInBucket(current))
                {
                    img_bucket.Source = Opers.LoadImage(File.ReadAllBytes("./Images/updatebucket.png"));
                }
                else
                {
                    img_bucket.Source = Opers.LoadImage(File.ReadAllBytes("./Images/bucket.png"));
                }
                if (Opers.IsHasOrder(current))
                {
                    img_order.Source = Opers.LoadImage(File.ReadAllBytes("./Images/updateorder.png"));
                }
                else
                {
                    img_order.Source = Opers.LoadImage(File.ReadAllBytes("./Images/order.png"));
                }
            }
                
            
        }

        private void accountBTN_Click(object sender, RoutedEventArgs e)
        {
            homeFrame
                .Navigate(new AccountPage());
        }

        private void bucketPageBTN_Click(object sender, RoutedEventArgs e)
        {
            homeFrame
                .Navigate(new BucketPage());
        }

        private void typesCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using(var db = new DbModelContainer())
            {
                if (typesCB.SelectedItem == null) return;
                var type = typesCB.SelectedItem.ToString();
                var search = searchTB.Text;
                if (type == "Все товары")
                {
                    var goods_null = db.GoodSet.ToList().Where(x => x.Name.Contains(search) || x.Type.Contains(search) || x.Info.Contains(search)).ToList();
                    var gpage1 = new GoodsPage();
                    gpage1.LoadGoods(goods_null);
                    homeFrame.Navigate(gpage1);
                    return;
                }
                var goods = db.GoodSet.Where(x => x.Type.Contains(type) && (x.Name.Contains(search) || x.Type.Contains(search) || x.Info.Contains(search))).ToList();
                var gpage = new GoodsPage();
                gpage.LoadGoods(goods);
                homeFrame.Navigate(gpage);
                

            }
        }

        private void searchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (var db = new DbModelContainer())
            {

                var type = typesCB.SelectedItem.ToString();
                var search = searchTB.Text;
                if (type == "Все товары")
                {
                    var goods_null = db.GoodSet.ToList().Where(x=>x.Name.Contains(search) || x.Type.Contains(search)||x.Info.Contains(search)).ToList();
                    var gpage1 = new GoodsPage();
                    gpage1.LoadGoods(goods_null);
                    homeFrame.Navigate(gpage1);
                    return;
                }
                var goods = db.GoodSet.Where(x => x.Type.Contains(type) && (x.Name.Contains(search) || x.Type.Contains(search) || x.Info.Contains(search))).ToList();
                var gpage = new GoodsPage();
                gpage.LoadGoods(goods);
                homeFrame.Navigate(gpage);


            }
        }

        private void orderBTN_Click(object sender, RoutedEventArgs e)
        {
            homeFrame.Navigate(new OrderPage(HomeWindow.currentUser));
        }
    }
}
