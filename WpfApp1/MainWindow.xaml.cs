using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
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
using WpfApp1.Properties;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   private TravelEntities _myconttext = new TravelEntities();
        private List<Tour> _tour;
        public MainWindow()
        {
            InitializeComponent();
            
            ListTours.ItemsSource = _myconttext.Tour.OrderBy(tour =>
tour.Name).ToList();
           
            List<Type> types = new List<Type>();
            types.Add(new Type() { Name = "Все типы" });
            types.AddRange(_myconttext.Type.OrderBy(t => t.Name).ToList());
            cmbFind.ItemsSource = types;
            this._tour = _myconttext.Tour.ToList();
          
        }

        private void txbFind_TextChanged(object sender, TextChangedEventArgs e)
        {
           string _findedName = txbFind.Text;
           _tour = _myconttext.Tour.OrderBy(t => t.Name).ToList();

            if (_findedName != "")
            {
                _tour = _tour.OrderBy(t => t.Name).Where(t =>
                t.Name.ToLower().Contains(_findedName)).ToList();
            }
            else
            {
                _tour = _myconttext.Tour.ToList();
            }
            ListTours.ItemsSource = _tour;
        }


        private void cmbFind_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFind.SelectedIndex == 0)
            {
                _tour = _myconttext.Tour.OrderBy(t => t.Name).ToList();
                _tour = (from t in _tour
                         select t).ToList();
                ListTours.ItemsSource = _tour;
            }
            else
            {
                _tour = _myconttext.Tour.OrderBy(t => t.Name).ToList();
                Type type = cmbFind.SelectedItem as Type;
                var _selectedType = type.Name;
                _tour = (from t in _tour
                         from tt in t.Type
                         where tt.Name == _selectedType
                         select t).ToList();
                ListTours.ItemsSource = _tour;
            }
            
        }

        private void chbFind_Unchecked(object sender, RoutedEventArgs e)
        {
            _tour = _myconttext.Tour.OrderBy(t => t.Name).ToList();
          
            Type type = cmbFind.SelectedItem as Type;
            _tour = (from t in _tour
                     select t).ToList();
            ListTours.ItemsSource = _tour;

        }

        private void chbFind_Checked(object sender, RoutedEventArgs e)
        {
            _tour = _myconttext.Tour.OrderBy(t => t.Name).ToList();
            Type type = cmbFind.SelectedItem as Type;
            _tour = (from t in _tour
                     where t.IsActual == true
                     select t).ToList();
            ListTours.ItemsSource = _tour;
        }

        private void chbwithImg_Unchecked(object sender, RoutedEventArgs e)
        {
            _tour = _myconttext.Tour.OrderBy(t => t.Name).ToList();
            Type type = cmbFind.SelectedItem as Type;
            _tour = (from t in _tour
                     select t).ToList();
            ListTours.ItemsSource = _tour; 
            ListTours.UpdateLayout();
        }

        private void chbwithImg_Checked(object sender, RoutedEventArgs e)
        {
            _tour = _myconttext.Tour.OrderBy(t => t.Name).ToList();
            Type type = cmbFind.SelectedItem as Type;
            _tour = (from t in _tour
                     select t).ToList();

            List<Tour> _tour2 = new List<Tour>();

            foreach (Tour item in _tour)
            {
                string workDirectory = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + @"..\..\..\..\" + item.ImdPath);
                if(File.Exists(workDirectory))
                {
                    _tour2.Add(item);
                }
            }
            ListTours.ItemsSource = _tour2;
            ListTours.UpdateLayout();
        }

        private void btnGoHotel_Click(object sender, RoutedEventArgs e)
        {
            Windows.DesktopHotels win = new Windows.DesktopHotels();
            win.ShowDialog();
        }
    }
}
