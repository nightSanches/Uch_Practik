using Airport.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics.Eventing.Reader;
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
    public partial class Raspisanye_add_edit : Window
    {
        public Raspisanye_add_edit()
        {
            InitializeComponent();
        }

        public string ssamolet;
        public string sdate;
        public string stime;
        public string splace_out;
        public string splace_in;
        public string sroute;
        public string sstoimost;

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            string connectionString = CurrentUser.connectString(CurrentUser.login, CurrentUser.password);
            string queryString = "SELECT Samolet_Kod FROM Planes";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<string> res = new List<string>();
                while (reader.Read())
                {
                    res.Add(reader["Samolet_Kod"].ToString());
                }
                foreach (string kod in res)
                {
                    samolet.Items.Add(kod);
                }
                connection.Close();
            }

            if (sdate != null)
            {
                samolet.Text = ssamolet.Trim();
                sdate.Trim();
                date.Text = sdate.Substring(0, sdate.Length - 8);
                time.Text = stime.Trim();
                place_out.Text = splace_out.Trim();
                place_in.Text = splace_in.Trim();
                route.Text = sroute.Trim();
                stoimost.Text = sstoimost.Trim();
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (samolet.Text == "" || samolet.Text == null)
            {
                MessageBox.Show("Не указан номер самолета", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(date.Text) || !Classes.Common.CheckRegex.Match("^\\d{2}\\.\\d{2}\\.\\d{4}$", date.Text))
            {
                MessageBox.Show("Неправильно указана дата вылета\nПример: 01.01.2001", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(time.Text) || !Classes.Common.CheckRegex.Match("\\b[0-2]?\\d:[0-5]\\d\\b", time.Text))
            {
                MessageBox.Show("Неправильно указано время\nПример: 13:15", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(stoimost.Text) || !Classes.Common.CheckRegex.Match("^\\d+$", stoimost.Text))
            {
                MessageBox.Show("Неправильно указана стоимость", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.DialogResult = true;
            }
        }

        public string Samolet_kod
        {
            get { return samolet.Text; }
        }
        public string Date
        {
            get { return date.Text; }
        }

        public string Time
        {
            get { return time.Text; }
        }

        public string Place_out
        {
            get { return place_out.Text; }
        }
        public string Place_in
        {
            get { return place_in.Text; }
        }
        public string Route
        {
            get {return route.Text; }
        }
        public string Stoimost
        {
            get { return stoimost.Text; }
        }
    }
}
