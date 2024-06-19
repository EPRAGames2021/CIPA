using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

public class DatabaseHelper
{
    // private readonly string _connectionString;

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Método para executar uma consulta SQL parametrizada
    public void ExecuteQuery(string query, SqlParameter[] parameters)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddRange(parameters);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    // Método para obter dados de uma consulta SQL parametrizada
    public SqlDataReader ExecuteQueryWithResult(string query, SqlParameter[] parameters)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddRange(parameters);
        connection.Open();
        return command.ExecuteReader(CommandBehavior.CloseConnection);
    }

    // Método para criptografar uma senha usando SHA-256
    public string EncryptPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}