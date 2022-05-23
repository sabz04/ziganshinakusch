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
    /// Логика взаимодействия для GoodsPage.xaml
    /// </summary>
    public partial class GoodsPage : Page
    {
        static Dictionary<Good, StackPanel> goodStackPanels = new Dictionary<Good, StackPanel>();
        Style tbstyle = Application.Current.FindResource("tbStandart3") as Style;
        Style btnstyle = Application.Current.FindResource("btnStyle1") as Style;
        public static GoodsPage goodPage;
        public GoodsPage()
        {
            InitializeComponent();
            if(goodPage != null)
                goodPage.LoadGoods(new List<Good>());
            goodPage = this;

        }

        public void LoadGoods(List<Good> goods)
        {
            goodsPanel.Children.Clear();
            using (var db = new DbModelContainer())
            {

                var user =
                    db.Users.FirstOrDefault(x => x.Id == HomeWindow.currentUser.Id);
                var img_like = new Image();
                foreach (var good in goods)
                {
                    if (good.Bucket != null)
                    {
                        if (good.Bucket.Id != user.Bucket.Id)
                        {
                            continue;
                        }
                    }


                    bool isSkip = false;
                    foreach (var key in goodStackPanels.Keys)
                    {
                        if (key.Id == good.Id)
                        {
                            if (Opers.recentlyDeleted.Any(x => x.Id == key.Id))
                            { 
                                goodStackPanels.Remove(key);
                                break;
                            }
                            if(key.Info != good.Info || key.Name != good.Name|| key.Type != good.Type|| key.File != good.File)
                            {
                                goodStackPanels.Remove(key);
                                break;
                            }    
                            goodsPanel.Children.Add(goodStackPanels[key]);
                            isSkip = true;
                            break;
                        }
                    }
                    if (isSkip)
                        continue;


                    if (user.Bucket.Good.Any(x => x.Id == good.Id))
                    {
                        img_like = new Image { Source = Opers.LoadImage(File.ReadAllBytes("./Images/liked.png")) };
                    }
                    else
                    {
                        img_like = new Image { Source = Opers.LoadImage(File.ReadAllBytes("./Images/like.png")) };
                    }
                    var stck = new StackPanel() { Margin = new Thickness(10) };
                    stck.Children.Add(new Image { Height = 200, Source = Opers.LoadImage(Opers.CompressByImageAlg(720,good.File)) });
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

                    var liked_btn = new Button { Height = 30, Style = btnstyle, Content = img_like, Name = "button" + good.Id };
                    var info_btn = new Button { Height = 30, Style = btnstyle, Content = img_info, Name = "but" + good.Id };
                    liked_btn.Click += Liked_btn_Click;
                    info_btn.Click += Info_btn_Click;

                    wrp.Children.Add(liked_btn);
                    wrp.Children.Add(info_btn);
                    stck.Children.Add(wrp);

                    goodStackPanels.Add(good, stck);
                    goodsPanel.Children.Add(stck);
                }
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

        private void Liked_btn_Click(object sender, RoutedEventArgs e)
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
                if (user.Bucket.Good.Any(x => x.Id == good.Id))
                {
                    user.Bucket.Good.Remove(good);
                    var img_good = (Image)(sender as Button).Content;
                    good.Bucket = null;
                    img_good.Source = Opers.LoadImage(File.ReadAllBytes("./Images/like.png"));
                    db.SaveChanges();
                    Opers.UpdateBucket(user);
                    HomeWindow.home.CheckUser();
                    return;
                }
                if(!Opers.AddGoodToBucket(user, good))
                {
                    MessageBox.Show("Ошибка при добавлении");
                    return;
                };
                var img = (Image)(sender as Button).Content;
                img.Source = Opers.LoadImage(File.ReadAllBytes("./Images/liked.png"));
                db.SaveChanges();
                Opers.UpdateBucket(user);

            }
            HomeWindow.home.CheckUser();
        }
    }
}
