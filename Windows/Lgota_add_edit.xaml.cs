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
    public partial class Lgota_add_edit : Window
    {
        public string Lgota_naim;
        public string Lgota_skidka;

        public Lgota_add_edit()
        {
            InitializeComponent();
        }
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (Lgota_naim != null || Lgota_skidka != null)
            {
                lgota_naim.Text = Lgota_naim.Trim();
                lgota_skidka.Text = Lgota_skidka.Trim();
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(lgota_naim.Text) || !Classes.Common.CheckRegex.Match("^[а-яА-ЯёЁ0-9\\s]+$", lgota_naim.Text))
            {
                MessageBox.Show("Неправильно указано наименование льготы", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(lgota_skidka.Text) || !Classes.Common.CheckRegex.Match("^[0][,][0-9]+$", lgota_skidka.Text))
            {
                MessageBox.Show("Неправильно указан размер скидки\nПример: 0,05 0,5", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.DialogResult = true;
            }
        }

        public string Naimenovanye
        {
            get { return lgota_naim.Text; }
        }
        public string Skidka
        {
            get { return lgota_skidka.Text; }
        }

    }
}
