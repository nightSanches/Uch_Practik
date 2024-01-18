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
            Rabotniki_add_edit taskWindow = new Rabotniki_add_edit();
            if (taskWindow.ShowDialog() == true)
            {
                DataRow row = table.NewRow();
                row[1] = taskWindow.Familia;
                row[2] = taskWindow.Imya;
                row[3] = taskWindow.Otchestvo;
                row[4] = Convert.ToDateTime(taskWindow.Date_birth);
                row[5] = Convert.ToDateTime(taskWindow.Date_employ);
                row[6] = taskWindow.Experience;
                row[8] = taskWindow.Pol;
                row[9] = taskWindow.Adress;
                row[10] = taskWindow.Town;
                row[11] = taskWindow.Phone;
                if (taskWindow.Job != "")
                {
                    string queryString = $"SELECT Job_kod FROM Jobs WHERE Job_naimenovanye = '{taskWindow.Job}'";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        string res = "";
                        while (reader.Read())
                        {
                            res = reader["Job_kod"].ToString();
                        }
                        row[7] = Convert.ToInt32(res);
                        connection.Close();
                    }
                }
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
            Rabotniki_add_edit taskWindow = new Rabotniki_add_edit();

            if ((DataRowView)tableGrid.SelectedItem != null)
            {
                DataRowView dataRowView = (DataRowView)tableGrid.SelectedItem;

                taskWindow.sfamilia = dataRowView["Rabotnik_familia"].ToString();
                taskWindow.simya = dataRowView["Rabotnik_imya"].ToString();
                taskWindow.sotchestvo = dataRowView["Rabotnik_otchestvo"].ToString();
                taskWindow.sdate_birth = dataRowView["Rabotnik_date_birth"].ToString();
                taskWindow.sdate_employ = dataRowView["Rabotnik_date_employment"].ToString();
                taskWindow.sexperience = dataRowView["Rabotnik_experience"].ToString();
                taskWindow.sjob = dataRowView["Rabotnik_job"].ToString();
                taskWindow.spol = dataRowView["Rabotnik_pol"].ToString();
                taskWindow.sadress = dataRowView["Rabotnik_adress"].ToString();
                taskWindow.stown = dataRowView["Rabotnik_town"].ToString();
                taskWindow.sphone = dataRowView["Rabotnik_phone"].ToString();
                if (taskWindow.ShowDialog() == true)
                {
                    dataRowView.BeginEdit();
                    dataRowView[1] = taskWindow.Familia;
                    dataRowView[2] = taskWindow.Imya;
                    dataRowView[3] = taskWindow.Otchestvo;
                    dataRowView[4] = Convert.ToDateTime(taskWindow.Date_birth);
                    dataRowView[5] = Convert.ToDateTime(taskWindow.Date_employ);
                    dataRowView[6] = taskWindow.Experience;
                    dataRowView[8] = taskWindow.Pol;
                    dataRowView[9] = taskWindow.Adress;
                    dataRowView[10] = taskWindow.Town;
                    dataRowView[11] = taskWindow.Phone;
                    if (taskWindow.Job != "")
                    {
                        string queryString = $"SELECT Job_kod FROM Jobs WHERE Job_naimenovanye = '{taskWindow.Job}'";
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand(queryString, connection);
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            string res = "";
                            while (reader.Read())
                            {
                                res = reader["Job_kod"].ToString();
                            }
                            dataRowView[7] = Convert.ToInt32(res);
                            connection.Close();
                        }
                    }
                    dataRowView.EndEdit();
                    UpdateDB();
                }
            }
        }

        private void backClick(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).frame.Navigate(new Pages.Tables());
        }
    }
}
