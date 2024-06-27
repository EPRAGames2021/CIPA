class Program
{
    static async Task Main(string[] args)
    {
        DatabaseHelper dbHelper = new DatabaseHelper();

        // Dados para inserir no Firestore
        var dataToInsert = new { Name = "John Doe", Email = "john.doe@example.com" };

        // Executar uma inserção
        string documentId = await dbHelper.ExecuteQuery("users", "insert", dataToInsert);
        Console.WriteLine($"Documento criado com ID: {documentId}");

        // Dados para atualizar no Firestore
        var dataToUpdate = new { Email = "new.email@example.com" };

        // Executar uma atualização
        string updatedDocumentId = await dbHelper.ExecuteQuery("users", "update", dataToUpdate, documentId);
        Console.WriteLine($"Documento atualizado com ID: {updatedDocumentId}");

        // Executar uma exclusão
        string deletedDocumentId = await dbHelper.ExecuteQuery("users", "delete", null, documentId);
        Console.WriteLine($"Documento excluído com ID: {deletedDocumentId}");

        // Obter dados de um documento com base em múltiplos campos
        var filters = new Dictionary<string, object>
        {
            { "Name", "John Doe" },
            { "Email", "new.email@example.com" }
        };

        var result = await dbHelper.ExecuteQueryWithResult("users", filters);

        if (result.Documents.Count > 0)
        {
            foreach (var doc in result.Documents)
            {
                Console.WriteLine($"Nome: {doc.GetValue<string>("Name")}, Email: {doc.GetValue<string>("Email")}");
            }
        }
        else
        {
            Console.WriteLine("Documento não encontrado!");
        }
    }
}
