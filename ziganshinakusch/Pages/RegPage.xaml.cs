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

namespace ziganshinakusch.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void submitBTN_Click(object sender, RoutedEventArgs e)
        {
            using(var db = new DbModelContainer())
            {
                var user =
                    db.Users.FirstOrDefault(x => x.Login == logTB.Text);
                if(user != null)
                {
                    MessageBox.Show("Пользователь с таким именем уже зарегистрирован.");
                    return;
                }
                db.Users.Add(new User
                {
                    Login = logTB.Text,
                    Password = passTB.Text,
                    Bucket = new Bucket() { Count=0, TotalPrice = 0},
                    
               });

                db.SaveChanges();
                MessageBox.Show("Регистрация успешна!");
            }
        }

        private void TextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainController
                .mainFrame.Navigate(new LogPage());
        }
    }
}
