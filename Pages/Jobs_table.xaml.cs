using Airport.Classes;
using Airport.Windows;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Airport.Pages
{
    public partial class Jobs_table : Page
    {
        DataTable table;
        SqlDataAdapter adapter;
        public Jobs_table()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM Jobs";
            table = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(CurrentUser.connectString(CurrentUser.login, CurrentUser.password));
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand("sp_InsertJobs", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@job_naimenovanye", SqlDbType.NChar, 100, "Job_naimenovanye"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@job_kod", SqlDbType.Int, 0, "Job_kod");
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
            Jobs_add_edit taskWindow = new Jobs_add_edit();
            if (taskWindow.ShowDialog() == true)
            {
                DataRow row = table.NewRow();
                row[1] = taskWindow.Naimenovanye;
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
            Jobs_add_edit taskWindow = new Jobs_add_edit();

            if ((DataRowView)tableGrid.SelectedItem != null)
            {
                DataRowView dataRowView = (DataRowView)tableGrid.SelectedItem;
                taskWindow.Job_naim = dataRowView["Job_naimenovanye"].ToString();
                if (taskWindow.ShowDialog() == true)
                {
                    dataRowView.BeginEdit();
                    dataRowView[1] = taskWindow.Naimenovanye;
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
