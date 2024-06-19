class Program
{
    static void Main(string[] args)
    {
        string connectionString = "sua_string_de_conexao";
        DatabaseHelper dbHelper = new DatabaseHelper(connectionString);

        // Exemplo de consulta parametrizada para inserção
        string insertQuery = "INSERT INTO Usuarios (Username, Password) VALUES (@Username, @Password, @City)";
        string username = "usuario_exemplo";
        string password = "senha_exemplo";
        string city = "Salvador";
        string encryptedPassword = dbHelper.EncryptPassword(password);

        SqlParameter[] insertParameters = {
            new SqlParameter("@Username", username),
            new SqlParameter("@Password", encryptedPassword),
            new SqlParameter("@City", city),

        };

        dbHelper.ExecuteQuery(insertQuery, insertParameters);

        // Exemplo de consulta parametrizada para leitura
        string selectQuery = "SELECT * FROM Usuarios WHERE Username = @Username";
        SqlParameter[] selectParameters = {
            new SqlParameter("@Username", username)
        };

        using (SqlDataReader reader = dbHelper.ExecuteQueryWithResult(selectQuery, selectParameters))
        {
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["ID"]}, Username: {reader["Username"]}, Password: {reader["Password"]}");
            }
        }

        // Exemplo de login com consulta
        string selectQuery = "SELECT * FROM Usuarios WHERE Username = @Username AND @Password LIMIT";
        SqlParameter[] selectParameters = {
            new SqlParameter("@Username", username)
            new SqlParameter("@Password", encryptedPassword)
        };

        using (SqlDataReader reader = dbHelper.ExecuteQueryWithResult(selectQuery, selectParameters))
        {
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["ID"]}, Username: {reader["Username"]}, Password: {reader["Password"]}");
            }
        }
    }
}