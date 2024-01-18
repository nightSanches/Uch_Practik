using Airport.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using Airport.Windows;

namespace Airport.Pages
{
    /// <summary>
    /// Логика взаимодействия для Raspisanye_table.xaml
    /// </summary>
    public partial class Raspisanye_table : Page
    {
        DataTable table;
        SqlDataAdapter adapter;
        string connectionString = CurrentUser.connectString(CurrentUser.login, CurrentUser.password);
        public Raspisanye_table()
        {
            InitializeComponent();
            if (CurrentUser.login != "superadmin")
            {
                add.Visibility = Visibility.Hidden;
                edit.Visibility = Visibility.Hidden;
                delete.Visibility = Visibility.Hidden;
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM Raspisanye_viletov";
            table = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand("sp_InsertVilety", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@samolet_kod", SqlDbType.Int, 0, "Samolet_kod"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@reis_date", SqlDbType.Date, 0, "Reis_date"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@reis_time", SqlDbType.NChar, 100, "Reis_time"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@reis_place_out", SqlDbType.NChar, 100, "Reis_place_out"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@reis_place_in", SqlDbType.NChar, 100, "Reis_place_in"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@reis_marshrut", SqlDbType.NChar, 200, "Reis_marshrut"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@reis_stoimost", SqlDbType.Float, 0, "Reis_stoimost"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@reis_kod", SqlDbType.Int, 0, "Reis_kod");
                parameter.Direction = ParameterDirection.Output;
                connection.Open();
                adapter.Fill(table);
                tableGrid.ItemsSource = table.DefaultView;
            }
            catch
            {
                MessageBox.Show("Ошибка запроса", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                connection?.Close();
            }
        }
        private void UpdateDB()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
            adapter.Update(table);
        }

        private void addClick(object sender, RoutedEventArgs e)
        {
            Raspisanye_add_edit taskWindow = new Raspisanye_add_edit();
            if (taskWindow.ShowDialog() == true)
            {
                DataRow row = table.NewRow();
                row[1] = taskWindow.Samolet_kod;
                row[2] = Convert.ToDateTime(taskWindow.Date);
                row[3] = taskWindow.Time;
                row[4] = taskWindow.Place_out;
                row[5] = taskWindow.Place_in;
                row[6] = taskWindow.Route;
                row[7] = taskWindow.Stoimost;
                table.Rows.Add(row);
                UpdateDB();
            }
        }

        private void deleteClick(object sender, RoutedEventArgs e)
        {
            if (tableGrid.SelectedItems != null)
            {
                for (int i = 0; i < tableGrid.SelectedItems.Count; i++)
                {
                    DataRowView dataRowView = tableGrid.SelectedItems[i] as DataRowView;
                    if (dataRowView != null)
                    {
                        DataRow datarow = (DataRow)dataRowView.Row;
                        datarow.Delete();
                    }
                }
            }
            UpdateDB();
        }

        private void editClick(object sender, RoutedEventArgs e)
        {
            Raspisanye_add_edit taskWindow = new Raspisanye_add_edit();

            if ((DataRowView)tableGrid.SelectedItem != null)
            {
                DataRowView dataRowView = (DataRowView)tableGrid.SelectedItem;
                taskWindow.ssamolet = dataRowView["Samolet_kod"].ToString();
                taskWindow.sdate = dataRowView["Reis_date"].ToString();
                taskWindow.stime = dataRowView["Reis_time"].ToString();
                taskWindow.splace_out = dataRowView["Reis_place_out"].ToString();
                taskWindow.splace_in = dataRowView["Reis_place_in"].ToString();
                taskWindow.sroute = dataRowView["Reis_marshrut"].ToString();
                taskWindow.sstoimost = dataRowView["Reis_stoimost"].ToString();
                if (taskWindow.ShowDialog() == true)
                {
                    dataRowView.BeginEdit();
                    dataRowView[1] = taskWindow.Samolet_kod;
                    dataRowView[2] = Convert.ToDateTime(taskWindow.Date);
                    dataRowView[3] = taskWindow.Time;
                    dataRowView[4] = taskWindow.Place_out;
                    dataRowView[5] = taskWindow.Place_in;
                    dataRowView[6] = taskWindow.Route;
                    dataRowView[7] = taskWindow.Stoimost;
                    dataRowView.EndEdit();
                    UpdateDB();
                }
            }
            else
            {
                MessageBox.Show("Для редактирования необходимо снчала выбрать запись", "Ошибка редактирования", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void backClick(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).frame.Navigate(new Pages.Tables());
        }
    }
}
