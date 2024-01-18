using Airport.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
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
    public partial class Table : Page
    {
        DataTable table;
        SqlDataAdapter adapter;
        public Table()
        {
            InitializeComponent();
        }
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM Lgoty";
            table = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection($@"Data Source=10.0.181.146;Initial Catalog=Airport;User Id = {CurrentUser.login};Password = {CurrentUser.password}");
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
                MessageBox.Show("Ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void updateClick(object sender, RoutedEventArgs e)
        {
            UpdateDB();
        }

        private void deleteClick(object sender, RoutedEventArgs e)
        {
            if (tableGrid.SelectedItems != null)
            {
                for (int i = 0; i <tableGrid.SelectedItems.Count; i++)
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

        }
    }
}
