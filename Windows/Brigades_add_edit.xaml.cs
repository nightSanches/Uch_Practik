using Airport.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// <summary>
    /// Логика взаимодействия для Brigades_add_edit.xaml
    /// </summary>
    public partial class Brigades_add_edit : Window
    {
        public string snumber;
        public string splane;
        public string srabotnik;
        public Brigades_add_edit()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            string connectionString = CurrentUser.connectString(CurrentUser.login, CurrentUser.password);
            string queryString = "SELECT Samolet_kod FROM Planes";
            string queryString2 = "SELECT Rabotnik_kod FROM Rabotniki";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<string> res = new List<string>();

                while (reader.Read())
                {
                    res.Add(reader["Samolet_kod"].ToString());
                }
                foreach (string kod in res)
                {
                    samolet.Items.Add(kod);
                }
                connection.Close();
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString2, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<string> res = new List<string>();

                while (reader.Read())
                {
                    res.Add(reader["Rabotnik_kod"].ToString());
                }
                foreach (string kod in res)
                {
                    rabotnik.Items.Add(kod);
                }
                connection.Close();
            }
            if (splane != null)
            {
                number.Text = snumber;
                samolet.Text = splane;
                rabotnik.Text = srabotnik;
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(number.Text) || !Classes.Common.CheckRegex.Match("^\\d+$", number.Text))
            {
                MessageBox.Show("Неправильно указан номер бригады", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(samolet.Text))
            {
                MessageBox.Show("Необходимо указать код самолета", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(rabotnik.Text))
            {
                MessageBox.Show("Необходимо указать код работника", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.DialogResult = true;
            }
        }

        public string Number
        {
            get { return number.Text; }
        }
        public string Plane
        {
            get { return samolet.Text; }
        }
        public string Rabotnik
        {
            get { return rabotnik.Text; }
        }
    }
}
