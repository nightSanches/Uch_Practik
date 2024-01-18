using Airport.Classes;
using Airport.Windows;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Airport.Pages
{
    public partial class Lgoty_table : Page
    {
        DataTable table;
        SqlDataAdapter adapter;
        public Lgoty_table()
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
            string sql = "SELECT * FROM Lgoty";
            table = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(CurrentUser.connectString(CurrentUser.login, CurrentUser.password));
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand("sp_InsertLgota", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@lgota_name", SqlDbType.NChar, 100, "Lgota_name"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@lgota_skidka", SqlDbType.Float, 0, "Lgota_skidka"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@lgota_kod", SqlDbType.Int, 0, "Lgota_kod");
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

        private void addClick(object sender, RoutedEventArgs e)
        {
            Lgota_add_edit taskWindow = new Lgota_add_edit();
            if (taskWindow.ShowDialog() == true)
            {
                DataRow row = table.NewRow();
                row[1] = taskWindow.Naimenovanye;
                row[2] = Convert.ToDouble(taskWindow.Skidka);
                table.Rows.Add(row);
                UpdateDB();
            }
        }

        private void editClick(object sender, RoutedEventArgs e)
        {
            Lgota_add_edit taskWindow = new Lgota_add_edit();

            if ((DataRowView)tableGrid.SelectedItem != null)
            {
                DataRowView dataRowView = (DataRowView)tableGrid.SelectedItem;
                taskWindow.Lgota_naim = dataRowView["Lgota_name"].ToString();
                taskWindow.Lgota_skidka = dataRowView["Lgota_skidka"].ToString();
                if (taskWindow.ShowDialog() == true)
                {
                    dataRowView.BeginEdit();
                    dataRowView[1] = taskWindow.Naimenovanye;
                    dataRowView[2] = Convert.ToDouble(taskWindow.Skidka);
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
