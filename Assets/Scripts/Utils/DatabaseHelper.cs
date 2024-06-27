using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using System.IO;

public class DatabaseHelper
{
    private FirestoreDb _firestoreDb;

    static DatabaseHelper()
    {
        // Carregar o arquivo de chave diretamente da pasta Resources
        string path = Path.Combine(Application.dataPath, "Firebase/ServiceAccount/serviceAccountKey.json");
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(path)
        });
    }

    public DatabaseHelper()
    {
        _firestoreDb = FirestoreDb.Create("cipa-d3fcb"); // Substitua "your-project-id" pelo ID do seu projeto Firebase
    }

    // Método para executar uma operação no Firestore (inserção/atualização/exclusão)
    public async Task<string> ExecuteQuery(string collection, string operation, object data = null, string documentId = null)
    {
        CollectionReference collectionRef = _firestoreDb.Collection(collection);

        switch (operation.ToLower())
        {
            case "insert":
                ValidateData(data); // Validação dos dados
                DocumentReference docRef = await collectionRef.AddAsync(data);
                return docRef.Id; // Retorna o ID do documento criado

            case "update":
                ValidateData(data); // Validação dos dados
                ValidateDocumentId(documentId); // Validação do ID do documento
                DocumentReference updateDocRef = collectionRef.Document(documentId);
                await updateDocRef.SetAsync(data, SetOptions.MergeAll);
                return documentId; // Retorna o ID do documento atualizado

            case "delete":
                ValidateDocumentId(documentId); // Validação do ID do documento
                DocumentReference deleteDocRef = collectionRef.Document(documentId);
                await deleteDocRef.DeleteAsync();
                return documentId; // Retorna o ID do documento excluído

            default:
                throw new ArgumentException("Invalid operation type. Use 'insert', 'update', or 'delete'.");
        }
    }

    // Método para obter dados de uma consulta no Firestore com base em múltiplos campos
    public async Task<QuerySnapshot> ExecuteQueryWithResult(string collection, Dictionary<string, object> filters)
    {
        ValidateFilters(filters); // Validação dos filtros
        CollectionReference collectionRef = _firestoreDb.Collection(collection);
        Query query = collectionRef;

        foreach (var filter in filters)
        {
            query = query.WhereEqualTo(filter.Key, filter.Value);
        }

        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        return snapshot;
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

    // Método para validar dados antes de enviá-los para o Firestore
    private void ValidateData(object data)
    {
        // Implementar a validação necessária para os dados
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data), "Data cannot be null");
        }

        // Adicionar validações adicionais conforme necessário
    }

    // Método para validar o ID do documento
    private void ValidateDocumentId(string documentId)
    {
        if (string.IsNullOrWhiteSpace(documentId))
        {
            throw new ArgumentException("Document ID cannot be null or empty", nameof(documentId));
        }

        // Adicionar validações adicionais conforme necessário
    }

    // Método para validar os filtros de consulta
    private void ValidateFilters(Dictionary<string, object> filters)
    {
        if (filters == null || filters.Count == 0)
        {
            throw new ArgumentException("Filters cannot be null or empty", nameof(filters));
        }

        foreach (var filter in filters)
        {
            if (string.IsNullOrWhiteSpace(filter.Key))
            {
                throw new ArgumentException("Filter keys cannot be null or empty", nameof(filters));
            }

            if (filter.Value == null)
            {
                throw new ArgumentNullException(nameof(filters), "Filter values cannot be null");
            }
        }
    }
}
