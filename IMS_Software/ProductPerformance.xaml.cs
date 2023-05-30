using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace COnnection_test
{
    /// <summary>
    /// Interaction logic for ProductPerformance.xaml
    /// </summary>
    public partial class ProductPerformance : Page
    {
        public string ConnectionString { get; set; }
        public ProductPerformance()
        {
            InitializeComponent();
            string server = "131.217.174.230";
            string database = "group6";
            string username = "group6";
            string password = "KIT506_Group6";
            string constring = "SERVER=" + server + ";DATABASE=" + database + ";"
                + "UID=" + username + ";" + "PASSWORD=" + password + ";";

            ConnectionString = constring;
        }

        private void BacktoMainPageCLicked(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow?.ChangeView(new ProductList());
        }

        private int GetProductperformance(int id)
        {
            int totalQuantity = 0;
            string query = "SELECT QTY " +
               "FROM Purchase " +
               "WHERE ProductID = @id " +
               "AND purchase_date >= @startDate "+
               "AND purchase_date <= @endDate ";
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                // Open the database connection
                connection.Open();

                // Create a SqlCommand object with the query and connection
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@startDate", new DateTime(2023,1,1));
                    command.Parameters.AddWithValue("@endDate", new DateTime(2023,6,1));

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        totalQuantity = reader.GetInt32(0);
                        return totalQuantity;
                        // Process the totalQuantity value as needed
                    }

                    reader.Close();

                }
            }
            return totalQuantity;
        }

        private int GetTotalQuantity(int id)
        {
            int sum = 0;
            string query1 = "SELECT Category FROM Product WHERE ProductID = @id";
            string query3 = "SELECT ProductID FROM Product WHERE Category = @category1";
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();

            List<int> idList = new List<int>();

            MySqlCommand command = new MySqlCommand(query1, connection);

            command.Parameters.AddWithValue("@id", id);

            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                string category = reader.GetString(0);
                reader.Close();

                using (MySqlCommand command1 = new MySqlCommand(query3, connection))
                {
                    command1.Parameters.AddWithValue("@category1", category);

                    // Execute the query
                    MySqlDataReader reader1 = command1.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader1);
                    //var p = reader.ToString();
                    foreach (DataRow dr in dt.Rows)
                    {
                        var p = dr["productID"].ToString();
                        idList.Add(int.Parse(p));
                    }
                    foreach (var item in idList)
                    {
                        sum += GetProductperformance(item);
                    }
                    return sum;

                }
            }


            return sum;

        }

        private void ProceedCLicked(object sender, RoutedEventArgs e)
        {
            try
            {
                int value = int.Parse(ID.Text);
                int totalquantity = GetProductperformance(value);
                int totalquantityCategory = GetTotalQuantity(value);
                decimal performance = (totalquantity / totalquantityCategory) * 100;
                Performance.Text = decimal.Round(performance, 2).ToString();
            }
            catch
            {
                Performance.Text = "No record";
            }
            
        }
    }
}
