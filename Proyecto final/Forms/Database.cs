using System.Data.SqlClient;

public class Database
{
    private readonly string connectionString =
        "Data Source=MSI\\SQLEXPRESS;Initial Catalog=db;Integrated Security=True;Encrypt=False";
    public SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
}