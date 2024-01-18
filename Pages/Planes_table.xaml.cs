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
    public partial class Planes_table : Page
    {
        DataTable table;
        SqlDataAdapter adapter;
        public Planes_table()
        {
            InitializeComponent();
        }
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM Planes";
            table = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(CurrentUser.connectString(CurrentUser.login, CurrentUser.password));
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand("sp_InsertPlanes", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@samolet_vipusk_year", SqlDbType.NChar, 100, "Samolet_vipusk_year"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@samolet_mesta", SqlDbType.Int, 0, "Samolet_mesta"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@samolet_gruzopodjem", SqlDbType.Int, 0, "Samolet_gruzopodjem"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@samolet_kod", SqlDbType.Int, 0, "Samolet_kod");
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
            Planes_add_edit taskWindow = new Planes_add_edit();
            if (taskWindow.ShowDialog() == true)
            {
                DataRow row = table.NewRow();
                row[1] = taskWindow.Vipusk_year;
                row[2] = taskWindow.Mesta;
                row[3] = taskWindow.Gruzopojem;
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
            Planes_add_edit taskWindow = new Planes_add_edit();

            if ((DataRowView)tableGrid.SelectedItem != null)
            {
                DataRowView dataRowView = (DataRowView)tableGrid.SelectedItem;
                taskWindow.Plane_year = dataRowView["Samolet_vipusk_year"].ToString();
                taskWindow.Plane_mesta = dataRowView["Samolet_mesta"].ToString();
                taskWindow.Plane_gruzopodjem = dataRowView["Samolet_gruzopodjem"].ToString();
                if (taskWindow.ShowDialog() == true)
                {
                    dataRowView.BeginEdit();
                    dataRowView[1] = taskWindow.Vipusk_year;
                    dataRowView[2] = taskWindow.Mesta;
                    dataRowView[3] = taskWindow.Gruzopojem;
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
