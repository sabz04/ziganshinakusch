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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        static byte[] _data = null;
        Good selectedGood=null;
        public AdminPage()
        {
            InitializeComponent();
            LoadGoods();
        }

        private void LoadGoods()
        {
            goodsGrid.Items.Clear();
            using (var db = new DbModelContainer())
            {
                db.GoodSet.ToList().ForEach(x => goodsGrid.Items.Add(
                    new TempGood { Name = x.Name, Price = x.Price, Id = x.Id, Info = x.Info, BucketId= x.BucketId, Type=x.Type , File= Opers.LoadImage(Opers.CompressByImageAlg(20, x.File)) }));

            }
        }

        private void addBTn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!Opers.AddNewGood(NameTB.Text, priceTB.Text, infoTB.Text, typeTB.Text, _data))
                {
                    return;
                }
                
                LoadGoods();
                HomeWindow.home.Refresh(false);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void saveBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedGood == null)
                    return;
                using (var db = new DbModelContainer())
                {
                    
                    
                    var current_good =
                        db.GoodSet.FirstOrDefault(x => x.Id == selectedGood.Id);
                    current_good.Info = infoTB.Text;
                    current_good.Price = int.Parse(priceTB.Text);
                    current_good.Name = NameTB.Text;
                    current_good.File = _data;
                    current_good.Type = typeTB.Text;    
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            LoadGoods();
            HomeWindow.home.Refresh(false);

        }

        private void deleteBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedGood == null) return;
                using (var db = new DbModelContainer())
                {
                    var current_good =
                            db.GoodSet.FirstOrDefault(x => x.Id == selectedGood.Id);
                    db.GoodSet.Remove(current_good);
                    db.SaveChanges();
                }
                LoadGoods();
                Refresh();
                HomeWindow.home.Refresh(false);

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }    
        }
        private void Refresh()
        {
            selectedGood = null;
            infoTB.Text = "";
            priceTB.Text = "";
            _data = null;
            NameTB.Text = "";
            goofIMG.Source = Opers.LoadImage(Opers.CompressByImageAlg(20,File.ReadAllBytes("./Images/no_image.png")));
        }
  
        private void goofIMG_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var data =
                Opers.GetByteFileFromExplorer();
            if(data == null)
            {
                _data = File.ReadAllBytes("./Images/no_image.png");
                return;
            }
            _data = data;
            goofIMG.Source = Opers.LoadImage(Opers.CompressByImageAlg(20,_data));
        }

        private void goodsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                using (var db = new DbModelContainer())
                {
                    if (goodsGrid.SelectedItem == null) return;
                    var selected = (TempGood)goodsGrid.SelectedItem;
                    var good =
                        db.GoodSet.FirstOrDefault(x => x.Id == selected.Id);
                    if (good == null)
                    {
                        MessageBox.Show("Ошибка!");
                        return;
                    }
                    selectedGood = good;
                    NameTB.Text = good.Name;
                    priceTB.Text = good.Price.ToString();
                    infoTB.Text = good.Info;
                    typeTB.Text = good.Type;
                    goofIMG.Source = Opers.LoadImage(good.File);
                }
            }
            catch (Exception ex){ MessageBox.Show(ex.Message); }
        }

        private void refreshBTN_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
    class TempGood {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public long Price { get; set; }
        public string Type { get; set; }
        public int? BucketId { get; set; }
        public BitmapImage File { get; set; }
    
    }
}
