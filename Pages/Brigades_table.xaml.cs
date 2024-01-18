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
using Airport.Classes;
using Airport.Windows;

namespace Airport.Pages
{
    /// <summary>
    /// Логика взаимодействия для Brigades_table.xaml
    /// </summary>
    public partial class Brigades_table : Page
    {
        public Brigades_table()
        {
            InitializeComponent();
        }
        DataTable table;
        SqlDataAdapter adapter;

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM Brigades";
            table = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(CurrentUser.connectString(CurrentUser.login, CurrentUser.password));
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand("sp_InsertBrigades", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@brigada_number", SqlDbType.Int, 0, "Brigada_number"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@samolet_kod", SqlDbType.Int, 0, "Samolet_kod"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@rabotnik_kod", SqlDbType.Int, 0, "Rabotnik_kod"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@brigada_kod", SqlDbType.Int, 0, "Brigada_kod");
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
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        private void UpdateDB()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
            adapter.Update(table);
        }

        private void addClick(object sender, RoutedEventArgs e)
        {
            Brigades_add_edit taskWindow = new Brigades_add_edit();
            if (taskWindow.ShowDialog() == true)
            {
                DataRow row = table.NewRow();
                row[1] = Convert.ToInt32(taskWindow.Number);
                row[2] = Convert.ToInt32(taskWindow.Plane);
                row[3] = Convert.ToInt32(taskWindow.Rabotnik);
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
            Brigades_add_edit taskWindow = new Brigades_add_edit();

            if ((DataRowView)tableGrid.SelectedItem != null)
            {
                DataRowView dataRowView = (DataRowView)tableGrid.SelectedItem;
                taskWindow.snumber = dataRowView["Brigada_number"].ToString();
                taskWindow.splane = dataRowView["Samolet_kod"].ToString();
                taskWindow.srabotnik = dataRowView["Rabotnik_kod"].ToString();
                if (taskWindow.ShowDialog() == true)
                {
                    dataRowView.BeginEdit();
                    dataRowView[1] = taskWindow.Number;
                    dataRowView[2] = taskWindow.Plane;
                    dataRowView[3] = taskWindow.Rabotnik;
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
