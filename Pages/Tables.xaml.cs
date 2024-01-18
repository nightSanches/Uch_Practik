using Airport.Classes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace Airport.Pages
{
    /// <summary>
    /// Логика взаимодействия для Tables.xaml
    /// </summary>
    public partial class Tables : Page
    {
        public Tables()
        {
            InitializeComponent();
            if (CurrentUser.login != "superadmin")
            {
                rabotniki.Visibility = Visibility.Hidden;
                jobs.Visibility = Visibility.Hidden;
                brigades.Visibility = Visibility.Hidden;
                vedomost.Visibility = Visibility.Hidden;
            }
        }

        private void Lgoty_OpenTable(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).frame.Navigate(new Pages.Lgoty_table());
        }

        private void exitClick(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Close();
        }

        private void Vedomost_OpenTable(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).frame.Navigate(new Pages.Vedomost_table());
        }

        private void Jobs_OpenTable(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).frame.Navigate(new Pages.Jobs_table());
        }

        private void Planes_OpenTable(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).frame.Navigate(new Pages.Planes_table());
        }

        private void Rabotniki_OpenTable(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).frame.Navigate(new Pages.Rabotniki_table());
        }
    }
}
