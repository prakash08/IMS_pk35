using MySql.Data.MySqlClient;
using Notification.Wpf;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace COnnection_test
{
    /// <summary>
    /// Interaction logic for AddEdit.xaml
    /// </summary>
    public partial class AddEdit : Page
    {
        public string ConnectionString { get; set; }
        public NotificationManager notificationManager;
        public AddEdit(int id)
        {
            InitializeComponent();
            notificationManager = new NotificationManager();
            string server = "131.217.174.230";
            string database = "group6";
            string username = "group6";
            string password = "KIT506_Group6";
            string constring = "SERVER=" + server + ";DATABASE=" + database + ";"
                + "UID=" + username + ";" + "PASSWORD=" + password + ";";

            ConnectionString = constring;
            LoadProductDetail(id);
        }

        private void LoadProductDetail(int id)
        {
            string query = "SELECT * FROM Product WHERE ProductID = @id";
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                // Open the database connection
                connection.Open();

                // Create a SqlCommand object with the query and connection
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ID.Text = reader.GetString(0);
                            PName.Text = reader.GetString(1);
                            Category.Text = reader.GetString(2);
                            Height.Text = reader.GetString(3);
                            Width.Text = reader.GetString(4);
                            Depth.Text = reader.GetString(5);
                            EnergyRating.Text = reader.GetString(6);
                            BasePrice.Text = reader.GetString(7);
                            MinPrice.Text = reader.GetString(8);
                            ListPrice.Text = reader.GetString(9);
                            Stock.Text = reader.GetString(10);
                            Weight.Text = reader.GetString(11);
                            Warranty.Text = reader.GetString(12);
                            HomeDelivery.IsChecked = reader.GetString(13) == "Yes" ? true:false ;
                        }
                    }

                }
            }
        }

        private void AddProductCLicked(object sender, RoutedEventArgs e)
        {
            if (!ValidateProductRegister())
                return;
            try
            {
                string query = "INSERT INTO Product (productID, Name, Category, Height," +
                    " Width, Depth,Energy_Rating,base_price," +
                    "min_price,list_price," +
                    "Stock,Weight,Warranty,HomeDelivery)" +
                    "VALUES (@Value1, @Value2, @Value3, @Value4, @Value5, @Value6, @Value7, @Value8, @Value9, @Value10, @Value11, @Value12, @Value13,@Value14);";
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    // Open the database connection
                    connection.Open();

                    // Create a SqlCommand object with the query and connection
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Set parameter values
                        command.Parameters.AddWithValue("@Value1", ID.Text);
                        command.Parameters.AddWithValue("@Value2", PName.Text);
                        command.Parameters.AddWithValue("@Value3", Category.Text);
                        command.Parameters.AddWithValue("@Value4", Height.Text);
                        command.Parameters.AddWithValue("@Value5", Width.Text);
                        command.Parameters.AddWithValue("@Value6", Depth.Text);
                        command.Parameters.AddWithValue("@Value7", EnergyRating.Text);
                        command.Parameters.AddWithValue("@Value8", BasePrice.Text);
                        command.Parameters.AddWithValue("@Value9", MinPrice.Text);
                        command.Parameters.AddWithValue("@Value10", ListPrice.Text);
                        command.Parameters.AddWithValue("@Value11", Stock.Text);
                        command.Parameters.AddWithValue("@Value12", Weight.Text);
                        command.Parameters.AddWithValue("@Value13", Warranty.Text);
                        command.Parameters.AddWithValue("@Value14", HomeDelivery.IsChecked);

                        // Execute the query
                        command.ExecuteNonQuery();
                        notificationManager.Show("Success", "Product added successfully", NotificationType.Success, "WindowArea");
                    }
                }
            }
            catch (Exception ex)
            {
                notificationManager.Show("Fail", "Invalid Input Credentials", NotificationType.Error, "WindowArea");
            }
        }

        private void CancelEditClicked(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow?.ChangeView(new ProductList());
        }

        private void EditSaveClicked(object sender, RoutedEventArgs e)
        {
            if (!ValidateProductRegister())
                return;

            //operation to save the edited product
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Define your SQL query with parameterized values
                    string query = "UPDATE Product SET Name = @Value2, Category = @Value3 , Height = @Value4, Width = @Value5, Depth = @Value6,Energy_Rating = @Value7," +
                        "base_price = @Value8, min_price= @Value9, list_price = @Value10, HomeDelivery = @Value11, Stock = @Value13,Weight = @Value12,Warranty = @Value14 WHERE productID = @Value1";

                    // Create a SqlCommand object with the query and the connection
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Set parameter values
                        command.Parameters.AddWithValue("@Value1", ID.Text);
                        command.Parameters.AddWithValue("@Value2", PName.Text);
                        command.Parameters.AddWithValue("@Value3", Category.Text);
                        command.Parameters.AddWithValue("@Value4", Height.Text);
                        command.Parameters.AddWithValue("@Value5", Width.Text);
                        command.Parameters.AddWithValue("@Value6", Depth.Text);
                        command.Parameters.AddWithValue("@Value7", EnergyRating.Text);
                        command.Parameters.AddWithValue("@Value8", BasePrice.Text);
                        command.Parameters.AddWithValue("@Value9", MinPrice.Text);
                        command.Parameters.AddWithValue("@Value10", ListPrice.Text);
                        command.Parameters.AddWithValue("@Value11", HomeDelivery.IsChecked);
                        command.Parameters.AddWithValue("@Value12", Weight.Text);
                        command.Parameters.AddWithValue("@Value13", Stock.Text);
                        command.Parameters.AddWithValue("@Value14", Warranty.Text);

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();
                        notificationManager.Show("Success", "Product edited successfully", NotificationType.Success, "WindowArea");
                    }
                }
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow?.ChangeView(new ProductList());
            }
            catch (Exception ex)
            {
                notificationManager.Show("Fail", "Invalid Input Credentials", NotificationType.Error, "WindowArea");
            }
        }

        private bool ValidateProductRegister()
        {
            try
            {
                if (string.IsNullOrEmpty(ID.Text) || string.IsNullOrEmpty(PName.Text) || string.IsNullOrEmpty(Category.Text) || string.IsNullOrEmpty(Height.Text)
                || string.IsNullOrEmpty(Width.Text) || string.IsNullOrEmpty(Depth.Text) || string.IsNullOrEmpty(EnergyRating.Text) || string.IsNullOrEmpty(BasePrice.Text)
                || string.IsNullOrEmpty(MinPrice.Text) || string.IsNullOrEmpty(ListPrice.Text) || string.IsNullOrEmpty(Weight.Text)
                || string.IsNullOrEmpty(Stock.Text))
                {
                    notificationManager.Show("Fail", "Field should not be empty", NotificationType.Error, "WindowArea");
                    return false;
                }

                else if (int.Parse(MinPrice.Text) > int.Parse(ListPrice.Text))
                {
                    notificationManager.Show("Fail", "Listed price should not be less than Minimum price", NotificationType.Error, "WindowArea");
                    return false;
                }
                else if (int.Parse(BasePrice.Text) > int.Parse(ListPrice.Text))
                {
                    notificationManager.Show("Fail", "Listed price should not be less than Base price", NotificationType.Error, "WindowArea");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                notificationManager.Show("Fail", "Invalid Input Credentials", NotificationType.Error, "WindowArea");
                return false;
            }
            
        }
    }


}
