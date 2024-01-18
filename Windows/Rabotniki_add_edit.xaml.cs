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
    public partial class Rabotniki_add_edit : Window
    {
        public string sDateTime;
        public string sFIO;
        public string sPassport;
        public string sReis;
        public string sKolychestvo;
        public string sLgota;
        public bool sBagage;
        public Rabotniki_add_edit()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            string connectionString = CurrentUser.connectString(CurrentUser.login, CurrentUser.password);
            string queryString = "SELECT Reis_kod FROM Raspisanye_viletov";
            string queryString2 = "SELECT Lgota_name FROM Lgoty";
            string queryString3 = $"SELECT Lgota_name FROM Lgoty WHERE Lgota_kod = {sLgota}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<string> res = new List<string>();

                while (reader.Read())
                {
                    res.Add(reader["Reis_kod"].ToString());
                }
                foreach (string kod in res)
                {
                    reis.Items.Add(kod);
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
                    res.Add(reader["Lgota_name"].ToString());
                }
                lgota.Items.Add(null);
                foreach (string kod in res)
                {
                    lgota.Items.Add(kod);
                }
                connection.Close();
            }
            if (sDateTime != null && sFIO != null && sPassport != null && sReis != null && sKolychestvo != null || sLgota != null || sBagage != false)
            {
                datetime.Text = sDateTime.Trim();
                fio.Text = sFIO.Trim();
                passport.Text = sPassport.Trim();
                reis.Text = sReis.Trim();
                kolych.Text = sKolychestvo.Trim();
                fio.Text = sFIO.Trim();
                if (sLgota != "")
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryString3, connection);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<string> res = new List<string>();

                        while (reader.Read())
                        {
                            res.Add(reader["Lgota_name"].ToString());
                        }
                        foreach (string kod in res)
                        {
                            lgota.Text = kod;
                        }
                        connection.Close();
                    }
                }
                bagage.IsChecked = sBagage;
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(datetime.Text) || !Classes.Common.CheckRegex.Match("^\\d{2}\\.\\d{2}\\.\\d{4} [0-9]+:[0-9]+:[0-9]+$", datetime.Text))
            {
                MessageBox.Show("Неправильно указана дата и время\nПример: 01.01.2001 12:34:56", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(fio.Text) || !Classes.Common.CheckRegex.Match("^[а-яА-ЯёЁ0-9\\s]+$", fio.Text))
            {
                MessageBox.Show("Неправильно указана ФИО", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(passport.Text) || !Classes.Common.CheckRegex.Match("^([0-9]{2}\\s{1}[0-9]{2} [0-9]{6})?$", passport.Text))
            {
                MessageBox.Show("Неправильно указаны пасспортные данные\nПример: 12 34 567890", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (reis.Text == "" || reis.Text == null)
            {
                MessageBox.Show("Не указан номер рейса", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(kolych.Text) || !Classes.Common.CheckRegex.Match("^[0-9]+$", kolych.Text))
            {
                MessageBox.Show("Неправильно указано количество билетов", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.DialogResult = true;
            }
        }

        public string DateTime
        {
            get { return datetime.Text; }
        }

        public string FIO
        {
            get { return fio.Text; }
        }
        public string Passport
        {
            get { return passport.Text; }
        }
        public string Reis
        {
            get { return reis.Text; }
        }
        public string Kolychestvo
        {
            get { return kolych.Text; }
        }
        public string Lgota
        {
            get { return lgota.Text; }
        }
        public bool Bagage
        {
            get { return (bool)bagage.IsChecked; }
        }
    }
}