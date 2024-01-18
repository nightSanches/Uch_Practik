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
    public partial class Rabotniki_add_edit : Window
    {
        public Rabotniki_add_edit()
        {
            InitializeComponent();
        }

        public string sfamilia;
        public string simya;
        public string sotchestvo;
        public string sdate_birth;
        public string sdate_employ;
        public string sexperience;
        public string sjob;
        public string spol;
        public string sadress;
        public string stown;
        public string sphone;

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            string connectionString = CurrentUser.connectString(CurrentUser.login, CurrentUser.password);
            string queryString = "SELECT Job_naimenovanye FROM Jobs";
            string queryString2 = $"SELECT Job_naimenovanye FROM Jobs WHERE Job_kod = {sjob}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<string> res = new List<string>();

                while (reader.Read())
                {
                    res.Add(reader["Job_naimenovanye"].ToString());
                }
                foreach (string kod in res)
                {
                    job.Items.Add(kod);
                }
                connection.Close();
            }

            if (sfamilia != null)
            {
                familia.Text = sfamilia.Trim();
                imya.Text = simya.Trim();
                otchestvo.Text = sotchestvo.Trim();
                sdate_birth.Trim();
                sdate_employ.Trim();
                date_birth.Text = sdate_birth.Substring(0, sdate_birth.Length - 8);
                date_employment.Text = sdate_employ.Substring(0, sdate_employ.Length - 8);
                experience.Text= sexperience.Trim();
                pol.Text = spol.Trim();
                adress.Text = sadress.Trim();
                town.Text = stown.Trim();
                phone.Text = sphone.Trim();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString2, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    List<string> res = new List<string>();

                    while (reader.Read())
                    {
                        res.Add(reader["Job_naimenovanye"].ToString());
                    }
                    foreach (string kod in res)
                    {
                        job.Text = kod;
                    }
                    connection.Close();
                }
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(familia.Text) || !Classes.Common.CheckRegex.Match("^[а-яА-ЯёЁ0-9\\s]+$", familia.Text))
            {
                MessageBox.Show("Неправильно указана фамилия", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(imya.Text) || !Classes.Common.CheckRegex.Match("^[а-яА-ЯёЁ0-9\\s]+$", imya.Text))
            {
                MessageBox.Show("Неправильно указано имя", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(string.IsNullOrEmpty(otchestvo.Text) || !Classes.Common.CheckRegex.Match("^[а-яА-ЯёЁ0-9\\s]+$", otchestvo.Text))
            {
                MessageBox.Show("Неправильно указано отчество", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(date_birth.Text) || !Classes.Common.CheckRegex.Match("^\\d{2}\\.\\d{2}\\.\\d{4}$", date_birth.Text))
            {
                MessageBox.Show("Неправильно указана дата рождения\nПример: 01.01.2001", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(date_employment.Text) || !Classes.Common.CheckRegex.Match("^\\d{2}\\.\\d{2}\\.\\d{4}$", date_employment.Text))
            {
                MessageBox.Show("Неправильно указана дата приема на работу\nПример: 01.01.2001", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(experience.Text) || !Classes.Common.CheckRegex.Match("^[0-9]+$", experience.Text))
            {
                MessageBox.Show("Неправильно указан стаж(опыт работы)", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(job.Text))
            {
                MessageBox.Show("Необходимо указать должность", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(pol.Text) || !Classes.Common.CheckRegex.Match("^м$|^ж$", pol.Text))
            {
                MessageBox.Show("Неправильно указан пол", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrEmpty(phone.Text) || !Classes.Common.CheckRegex.Match("^((\\+7|7|8)+([0-9]){10})$", phone.Text))
            {
                MessageBox.Show("Неправильно указан номер телефона", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.DialogResult = true;
            }
        }

        public string Familia
        {
            get { return familia.Text; }
        }

        public string Imya
        {
            get { return imya.Text; }
        }
        public string Otchestvo
        {
            get { return otchestvo.Text; }
        }
        public string Date_birth
        {
            get { return date_birth.Text;}
        }

        public string Date_employ
        {
            get { return date_employment.Text;}
        }

        public string Experience
        {
            get {return experience.Text;}
        }

        public string Job
        {
            get { return job.Text;}
        }
        public string Pol
        {
            get { return pol.Text;}
        }
        public string Adress
        {
            get { return adress.Text;}
        }
        public string Town
        {
            get { return town.Text;}
        }
        public string Phone
        {
            get { return phone.Text;}
        }
    }
}