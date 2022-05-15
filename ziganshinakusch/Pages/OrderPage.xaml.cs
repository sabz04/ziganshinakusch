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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        Style tbstyle = Application.Current.FindResource("tbStandart4") as Style;
        Style btnstyle = Application.Current.FindResource("btnStyle1") as Style;
        public static User user;
        public OrderPage()
        {
            InitializeComponent();
            
        }
        public OrderPage(User cuser)
        {
            InitializeComponent();
            user = cuser;
            LoadOrders();
        }
        private void LoadOrders()
        {
            using (var db = new DbModelContainer())
            {
                orderPanel.Children.Clear();
                var current =
                    db.Users.FirstOrDefault(x => x.Id == user.Id);
                foreach(var order in current.Bucket.Order)
                {
                    var stck = new StackPanel() { HorizontalAlignment=HorizontalAlignment.Center, VerticalAlignment= VerticalAlignment.Center};
                    stck.Children.Add(new TextBlock
                    {
                        Text = $"Заказ: {order.Id}\nДата заказа: {order.Date}\nСписок товаров:" + order.Items,
                        Margin = new Thickness(10),

                        Style = tbstyle
                    });
                    var img_del = new Image { Source = Opers.LoadImage(File.ReadAllBytes("./Images/delete.png")) };
                    var btn = new Button
                    {
                        Content = img_del,
                        Style = btnstyle,
                        Height = 30,
                        Name = $"btn{order.Id}"
                    };
                    btn.Click += Btn_Click;
                    stck.Children.Add(btn);
                    orderPanel.Children.Add(stck);
                }
                HomeWindow.home.CheckUser();
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            var id = int.Parse((sender as Button).Name.Substring(3, (sender as Button).Name.Length - 3));
            using(var db = new DbModelContainer())
            {
                var order=
                    db.OrderSet.FirstOrDefault(x=>x.Id == id);
                if (order == null) return;
                var us =
                    db.Users.FirstOrDefault(x => x.Id == HomeWindow.currentUser.Id);
                if (us == null) return;
                us.Bucket.Order.Remove(order);
                db.SaveChanges();
                LoadOrders();
            }
            HomeWindow.home.CheckUser();
        }
    }
}
