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
    /// Логика взаимодействия для LogPage.xaml
    /// </summary>
    public partial class LogPage : Page
    {
        public LogPage()
        {
            InitializeComponent();
        }

        private void submitBTN_Click(object sender, RoutedEventArgs e)
        {
            var user = Opers.LogMethod(logTB.Text, passTB.Text);
            if(user == null) { return; }
            var home =
                    new HomeWindow(user);
            home.Show();
            MainWindow.mainController
                .Close();
            
        }

        private void TextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainController
                .mainFrame.Navigate(new RegPage());
        }
    }
}
