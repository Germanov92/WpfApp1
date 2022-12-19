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

namespace WpfApp1.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddHotel.xaml
    /// </summary>
    public partial class AddHotel : Window
    {
        private List<string> countries = new List<string>();
        private TravelEntities _myconttext;
        private DesktopHotels _window;
        public AddHotel(TravelEntities context,DesktopHotels dh)
        {
            InitializeComponent();
            this._myconttext = context;
            this._window = dh;
            foreach (Country item in _myconttext.Country) countries.Add(item.Name);
            cboxCountry.ItemsSource = countries;
           


        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            bool isint = Int32.TryParse(tbStars.Text, out int res);
            
            bool exists = _myconttext.Hotel.Where(i=>i.Name==tbName.Text).ToList().Count()>0;
            if(tbName.Text.Length > 0&&tbDescr.Text.Length>0&&isint&&res>0&&res<6&&tbDescr.LineCount>1&&cboxCountry.SelectedIndex>-1&&!exists)
            {
                _myconttext.Hotel.AddObject(new Hotel()
                {
                    Name = tbName.Text,
                    CountOfStars = Convert.ToInt32(tbStars.Text),
                    Description = tbDescr.Text,
                    Country = _myconttext.Country.Where(i => i.Name == (cboxCountry.SelectedItem.ToString())).First(),

                });
                _myconttext.SaveChanges();
                _window.refreshHotel();
                this.Close();
            }
            else
            {
                MessageBox.Show("Отель не добавлен. Все поля должны быть заполнены, число звезд - от 0 до 5, имя отеля уникальное, а описание более 1 строки!");
            }
          
        }
    }
}
