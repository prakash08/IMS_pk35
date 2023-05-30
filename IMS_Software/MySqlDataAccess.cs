using MySql.Data.MySqlClient;
using System.Data.SqlClient;

public class MySqlDataAccess
{
    private string connectionString;

    public MySqlDataAccess(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }
}
