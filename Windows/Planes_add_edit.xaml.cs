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

namespace Airport.Windows
{
    public partial class Planes_add_edit : Window
    {
        public string Plane_year;
        public string Plane_mesta;
        public string Plane_gruzopodjem;
        public Planes_add_edit()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (Plane_year != null && Plane_mesta != null && Plane_gruzopodjem != null)
            {
                year_vipusk.Text = Plane_year.Trim();
                mesta.Text = Plane_mesta.Trim();
                gruzopodjemnost.Text = Plane_gruzopodjem.Trim();
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(year_vipusk.Text) || !Classes.Common.CheckRegex.Match("^[0-9]{4}$", year_vipusk.Text))
            {
                MessageBox.Show("Неправильно указан год выпуска самолета", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(mesta.Text) || !Classes.Common.CheckRegex.Match("^[0-9]+$", mesta.Text))
            {
                MessageBox.Show("Неправильно указано количество посадочных мест", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(gruzopodjemnost.Text) || !Classes.Common.CheckRegex.Match("^[0-9]+$", gruzopodjemnost.Text))
            {
                MessageBox.Show("Неправильно указана грузоподъемность", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.DialogResult = true;
            }
        }

        public string Vipusk_year
        {
            get { return year_vipusk.Text; }
        }

        public string Mesta
        {
            get { return mesta.Text; }
        }

        public string Gruzopojem
        {
            get { return gruzopodjemnost.Text; }
        }
    }
}
