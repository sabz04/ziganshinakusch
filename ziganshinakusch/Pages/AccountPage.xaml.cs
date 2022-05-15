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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ziganshinakusch.Model;
using ziganshinakusch.Windows;

namespace ziganshinakusch.Pages
{
    /// <summary>
    /// Логика взаимодействия для AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        public AccountPage()
        {
            InitializeComponent();
            LoadUser();
        }

        private void LoadUser()
        {
            try
            {
                var us =
                HomeWindow.currentUser;
                logTB.Text = us.Login;
                passTB.Text = us.Password;
                emailTB.Text = us.Email;
                phoneTB.Text = us.Phone;
                cardTB.Text = us.CardNumber;
                fullnameTB.Text = us.FullName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void submitBTN_Click(object sender, RoutedEventArgs e)
        {
            using(var db = new DbModelContainer())
            {
                var user = 
                    db.Users.FirstOrDefault(u => u.Id == HomeWindow.currentUser.Id);
                if (user == null) return;
                user.Email = emailTB.Text;
                user.Phone = phoneTB.Text;
                user.Password = passTB.Text;
                user.Login = logTB.Text;
                user.CardNumber = cardTB.Text;
                user.FullName = fullnameTB.Text;
                db.SaveChanges();
                HomeWindow.currentUser = user;
                HomeWindow.home.CheckUser();
                MessageBox.Show("Изменения прошли успешно.");
            }
        }

        private void exitBTN_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            HomeWindow.currentUser = null;
            HomeWindow.home.Close();
            
        }

        private void goodsBTN_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow.home.homeFrame
                .Navigate(new GoodsPage());
        }
    }
}
