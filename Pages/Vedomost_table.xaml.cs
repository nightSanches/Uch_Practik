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
    public partial class Vedomost_table : Page
    {
        DataTable table;
        SqlDataAdapter adapter;
        string connectionString = CurrentUser.connectString(CurrentUser.login, CurrentUser.password);
        public Vedomost_table()
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
            string sql = "SELECT * FROM Vedomost_prodaj";
            table = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand("sp_InsertVedomost", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@prodaja_date_and_time", SqlDbType.DateTime, 0, "Prodaja_date_and_time"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@prodaja_fio", SqlDbType.NChar, 100, "Prodaja_fio"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@prodaja_passport", SqlDbType.NChar, 100, "Prodaja_passport"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@reis_kod", SqlDbType.Int, 0, "Reis_kod"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@prodaja_kolychestvo", SqlDbType.Int, 0, "Prodaja_kolychestvo"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@prodaja_lgota", SqlDbType.Int, 0, "Prodaja_lgota"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@prodaja_bagage", SqlDbType.Bit, 0, "Prodaja_bagage"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@prodaja_stoimost", SqlDbType.Float, 0, "Prodaja_stoimost"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@prodaja_kod", SqlDbType.Int, 0, "Prodaja_kod");
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
            Vedomost_add_edit taskWindow = new Vedomost_add_edit();

            if ((DataRowView)tableGrid.SelectedItem != null)
            {
                DataRowView dataRowView = (DataRowView)tableGrid.SelectedItem;
                taskWindow.sDateTime = dataRowView["Prodaja_date_and_time"].ToString();
                taskWindow.sFIO = dataRowView["Prodaja_fio"].ToString();
                taskWindow.sPassport = dataRowView["Prodaja_passport"].ToString();
                taskWindow.sReis = dataRowView["Reis_kod"].ToString();
                taskWindow.sKolychestvo = dataRowView["Prodaja_kolychestvo"].ToString();
                taskWindow.sLgota = dataRowView["Prodaja_lgota"].ToString();
                taskWindow.sBagage = Convert.ToBoolean(dataRowView["Prodaja_bagage"]);
                if (taskWindow.ShowDialog() == true)
                {
                    dataRowView.BeginEdit();
                    dataRowView[1] = Convert.ToDateTime(taskWindow.DateTime);
                    dataRowView[2] = taskWindow.FIO;
                    dataRowView[3] = taskWindow.Passport;
                    dataRowView[4] = Convert.ToInt32(taskWindow.Reis);
                    dataRowView[5] = Convert.ToInt32(taskWindow.Kolychestvo);
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
                            dataRowView[6] = Convert.ToInt32(res);
                            connection.Close();
                        }
                    }
                    else
                    {
                        dataRowView[6] = DBNull.Value;
                    }
                    dataRowView[7] = Convert.ToBoolean(taskWindow.Bagage);

                    string queryString3 = $"SELECT Reis_stoimost FROM Raspisanye_viletov WHERE Reis_kod = {dataRowView[4]}";
                    string queryString4 = $"SELECT Lgota_skidka FROM Lgoty WHERE Lgota_kod = {dataRowView[6]}";
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
                            stoimost = Convert.ToDouble(st) * Convert.ToDouble(dataRowView[5]);
                        }
                        connection.Close();
                    }
                    if (dataRowView[6].ToString() != "")
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
                    dataRowView[8] = Convert.ToDouble(stoimost);
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