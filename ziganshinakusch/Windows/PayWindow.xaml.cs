using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для PayWindow.xaml
    /// </summary>
    public partial class PayWindow : Window
    {
        public static User user;
        public PayWindow()
        {
            InitializeComponent();
            
        }
        public PayWindow(User cuser)
        {
            InitializeComponent();
            user = cuser;
            var items = "";
            user.Bucket.Good.ToList().ForEach(x => items += $"\n{x.Name}");
            checkList.Items.Add("Список товаров: " + items);
            checkList.Items.Add("Время:" + DateTime.Now.ToString());
        }

        private void acceptBTN_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DbModelContainer())
            {
                var curUser =
                    db.Users.FirstOrDefault(x => x.Id == user.Id);
                var items = "";
                
                curUser.Bucket.Good.ToList().ForEach(x => items += $"\n{x.Name}");
                curUser.Bucket.Order.Add(new Order()
                {
                    Date = DateTime.Now.ToString(),
                    Items = items
                });
                curUser.Bucket.Good.Clear();
                db.SaveChanges();
                Opers.UpdateBucket(curUser);
                HomeWindow.home.homeFrame.Navigate(new BucketPage());
                MessageBox.Show("Заказ создан.");
                this.Hide();
                HomeWindow.home.CheckUser();
            }
        }
    }
}
