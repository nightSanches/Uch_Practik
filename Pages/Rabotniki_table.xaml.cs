using Airport.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Airport.Windows;

namespace Airport.Pages
{
    public partial class Rabotniki_table : Page
    {
        DataTable table;
        SqlDataAdapter adapter;
        string connectionString = CurrentUser.connectString(CurrentUser.login, CurrentUser.password);
        public Rabotniki_table()
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
            string sql = "SELECT * FROM Rabotniki";
            table = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand("sp_InsertRabotniki", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@rabotnik_familia", SqlDbType.NChar, 100, "Rabotnik_familia"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@rabotnik_imya", SqlDbType.NChar, 100, "Rabotnik_imya"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@rabotnik_otchestvo", SqlDbType.NChar, 100, "Rabotnik_otchestvo"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@rabotnik_date_birth", SqlDbType.Date, 0, "Rabotnik_date_birth"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@rabotnik_date_employment", SqlDbType.Date, 0, "Rabotnik_date_employment"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@rabotnik_experience", SqlDbType.Int, 0, "Rabotnik_experience"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@rabotnik_job", SqlDbType.Int, 0, "Rabotnik_job"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@rabotnik_pol", SqlDbType.NChar, 100, "Rabotnik_pol"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@rabotnik_adress", SqlDbType.NChar, 200, "Rabotnik_adress"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@rabotnik_town", SqlDbType.NChar, 100, "Rabotnik_town"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@rabotnik_phone", SqlDbType.NChar, 100, "Rabotnik_phone"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@rabotnik_kod", SqlDbType.Int, 0, "Rabotnik_kod");
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
            Vedomost_add_edit taskWindow = new Vedomost_add_edit();
            if (taskWindow.ShowDialog() == true)
            {
                DataRow row = table.NewRow();
                row[1] = Convert.ToDateTime(taskWindow.DateTime);
                row[2] = taskWindow.FIO;
                row[3] = taskWindow.Passport;
                row[4] = Convert.ToInt32(taskWindow.Reis);
                row[5] = Convert.ToInt32(taskWindow.Kolychestvo);
                if (taskWindow.Lgota != "")
                {
                    string queryString = $"SELECT Lgota_kod FROM Lgoty WHERE Lgota_name = '{taskWindow.Lgota}'";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        string res = "";
                        while (reader.Read())
                        {
                            res = reader["Lgota_kod"].ToString();
                        }
                        row[6] = Convert.ToInt32(res);
                        connection.Close();
                    }
                }
                row[7] = Convert.ToBoolean(taskWindow.Bagage);

                string queryString3 = $"SELECT Reis_stoimost FROM Raspisanye_viletov WHERE Reis_kod = {row[4]}";
                string queryString4 = $"SELECT Lgota_skidka FROM Lgoty WHERE Lgota_kod = {row[6]}";
                double stoimost = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString3, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    List<string> res = new List<string>();
                    while (reader.Read())
                    {
                        res.Add(reader["Reis_stoimost"].ToString());
                    }
                    foreach (string st in res)
                    {
                        stoimost = Convert.ToDouble(st) * Convert.ToDouble(row[5]);
                    }
                    connection.Close();
                }
                if (row[6].ToString() != "")
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryString4, connection);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        List<string> res = new List<string>();
                        while (reader.Read())
                        {
                            res.Add(reader["Lgota_skidka"].ToString());
                        }
                        foreach (string st in res)
                        {
                            stoimost = stoimost - (stoimost * Convert.ToDouble(st));
                        }
                        connection.Close();
                    }
                }
                if (taskWindow.Bagage == true)
                {
                    stoimost += 300;
                }
                row[8] = Convert.ToDouble(stoimost);
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

        }

        private void backClick(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).frame.Navigate(new Pages.Tables());
        }
    }
}
