using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для EditInfoHotel.xaml
    /// </summary>
    /// 

    public partial class EditInfoHotel : Window
    {
      
        private List<string> countries = new List<string>();
        TravelEntities _myconttext;
        string oldname= string.Empty;
        string oldDescr= string.Empty;
        string oldstars= string.Empty;
        string oldCountry= string.Empty;
        Hotel hotel;
        public EditInfoHotel(Hotel nhotel,Object o, TravelEntities _mycon)
        {

            hotel = (o as Button).DataContext as Hotel;
            InitializeComponent();
            _myconttext = _mycon;
            tbName.Text = hotel.Name;
            tbDescr.Text = hotel.Description;
            tbStars.Text=hotel.CountOfStars.ToString();

            foreach (Country item in _myconttext.Country)countries.Add(item.Name);
          
            cboxCountry.ItemsSource = countries;
            cboxCountry.SelectedItem = hotel.Country.Name;
            
            //этот код нужен для сохранения старых значений если юзер решил отменить редактирование
            oldname=tbName.Text;
            oldDescr=tbDescr.Text;
            oldstars=tbStars.Text;
            oldCountry=hotel.Country.Name;
            
        }
       

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(hotel.Name, "Вы хотите удалить данный отель?",
                    MessageBoxButton.YesNo);



            List<Tour> actual = _myconttext.Tour.Where(i=>i.IsActual).ToList();
            List<Tour> htour = hotel.Tour.ToList();
            bool candelete = false;
            foreach(Tour t in htour)
            {
                if (actual.Contains(t)) candelete = true;
                break;
            }
           
            if (result == MessageBoxResult.Yes&&candelete)
            {
                _myconttext.Hotel.DeleteObject(hotel);
                _myconttext.SaveChanges();
                this.Close();
            }
            else { MessageBox.Show("Этот отель является актуальным для туров и вы не можете его удалить."); }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

            if (Int32.TryParse(tbStars.Text,out int stars)) { if (tbName.Text.Length > 0 &&stars>0&&stars<6&& cboxCountry.SelectedIndex > -1) {
                    var h = _myconttext.Hotel.Where(b => b.Name == tbName.Text);
                    if (h.Count()<2||hotel.Equals(h))//если такого отеля нет или отель с таким именем - это обьект идентичный тому что мы редактируем уже.ы
                    {
                        hotel.Name = tbName.Text;
                        hotel.Description = tbDescr.Text;
                        hotel.CountOfStars = Convert.ToInt32(tbStars.Text);
                        hotel.Country = _myconttext.Country.Where(c => c.Name == cboxCountry.SelectedItem.ToString()).First();
                        _myconttext.SaveChanges();
                        MessageBox.Show("Выбранный отель успешно отредактирован.");
                        
                    }
                    else { MessageBox.Show("Такой отель уже существует!"); }
                   
                }
            }
            else
            {
                MessageBox.Show("Данные введены некорректно или некоторые обязательные поля не заполнены.");
            }
        }
    }
}
