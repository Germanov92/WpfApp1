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
    /// Логика взаимодействия для DesktopHotels.xaml
    /// </summary>
    public partial class DesktopHotels : Window
    {
        public static TravelEntities _mycontext = new TravelEntities();
       
        private Hotel _hotel; //selected hotel
        private int _currentPage = 1; //current page
        private int _maxPage = 0; //max page
        public DesktopHotels()
        {
            InitializeComponent();
            dgHotel.ItemsSource = _mycontext.Hotel.ToList();
            refreshHotel();
        }
        public void refreshHotel()
        {
            dgHotel.ItemsSource = _mycontext.Hotel.OrderBy(h => h.Name).ToList();
            _maxPage = _mycontext.Hotel.ToList().Count / 10 + 1; ;
            var _listHotel = _mycontext.Hotel.ToList().Skip((_currentPage - 1) *
            10).Take(10).ToList();
            pages.Content = " of " + _maxPage.ToString();
            txtCurrentPage.Text = _currentPage.ToString();
            dgHotel.ItemsSource = _listHotel;
        }
        private void btnEditHotel_Click(object sender, RoutedEventArgs e)
        {

            EditInfoHotel editwindow=new EditInfoHotel(_hotel, sender,_mycontext);
            editwindow.ShowDialog();
            refreshHotel();

         }

        private void btnToFist_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            refreshHotel();
        }

      

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage - 1 < 1) { return; };
            _currentPage--;
            refreshHotel();
        }

        private void txtCurrentPage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_currentPage > 0 && _currentPage < _maxPage)
            {
                try
                {
                    _currentPage = Convert.ToInt32(txtCurrentPage.Text);
                    refreshHotel();
                }
                catch { }
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage + 1 > _maxPage) { return; };
            _currentPage++;
            refreshHotel();
        }

        private void btnToLast_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = _maxPage;
            refreshHotel();
        }

        private void btnGoTour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.ShowDialog();
        }

        private void dgHotel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _hotel = (Hotel)dgHotel.CurrentItem;
        }

        private void BtnAddHotel_Click(object sender, RoutedEventArgs e)
        {
            AddHotel addHotel = new AddHotel(_mycontext, this);
            addHotel.ShowDialog();
        }
    }
}
