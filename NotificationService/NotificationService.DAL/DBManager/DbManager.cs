using Microsoft.Data.SqlClient;

namespace NotificationService.DAL.DBManager;
public class DbManager : IDbManager
{
    private string connectionString;
    private string connectionStringWithoutDB;
    private string scriptPath;
    public DbManager(string connectionString, string connectionStringWithoutDB, string scriptPath)
    {
        this.connectionString = connectionString;
        this.connectionStringWithoutDB = connectionStringWithoutDB;
        this.scriptPath = scriptPath;
    }

    public async Task CreateTableAsync()
    {
        string createDatabaseSql = "CREATE DATABASE NotificationApplicationDb;";
        await ExecuteNonQuery(createDatabaseSql, connectionStringWithoutDB);
        string createTableSql = GetSqlFromScript("CREATE TABLE Events");
        await ExecuteNonQuery(createTableSql, connectionString);
    }

    public async Task DropTableAsync()
    {
        string dropTableSql = GetSqlFromScript("DROP TABLE Events");
        await ExecuteNonQuery(dropTableSql, connectionString);
    }

    private async Task ExecuteNonQuery(string sql, string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    private string GetSqlFromScript(string scriptName)
    {
        string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, scriptPath);
        string scriptContent = File.ReadAllText(filePath);
        string[] scripts = scriptContent.Split(new string[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string script in scripts)
        {
            if (script.Trim().StartsWith(scriptName))
            {
                return script.Trim();
            }
        }

        throw new Exception($"Script '{scriptName}' not found in the file.");
    }
}
