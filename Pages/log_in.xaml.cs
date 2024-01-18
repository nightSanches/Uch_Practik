using Airport.Classes;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Airport.Pages
{
    public partial class log_in : Page
    {
        public log_in()
        {
            InitializeComponent();
        }

        private void bth_log_in(object sender, RoutedEventArgs e)
        {
            string loginUser = tb_login.Text;
            string passwordUser = tb_password.Text;
            try
            {
                SqlConnection sqlConnection = new SqlConnection(CurrentUser.connectString(loginUser, passwordUser));
                CurrentUser.login = loginUser;
                CurrentUser.password = passwordUser;
                sqlConnection.Open();
                sqlConnection.Close();

                ((MainWindow)Application.Current.MainWindow).frame.Navigate(new Pages.Tables());
            }
            catch
            {
                MessageBox.Show("Введённый логин или пароль неправильный", "Соединение с сервером", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
