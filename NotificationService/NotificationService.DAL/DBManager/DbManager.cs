using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace NotificationService.DAL.DBManager;
public class DbManager : IDbManager
{
    private string connectionString;
    private string connectionStringWithoutDB;
    private string scriptPath;
    public DbManager(IConfiguration configuration)
    {
        this.connectionString = configuration.GetConnectionString("DBConnection") ?? throw new ArgumentException("Connection string 'DBConnection' is missing or empty in configuration.");
        this.connectionStringWithoutDB = configuration.GetConnectionString("DBConnectionWithoutDB") ?? throw new ArgumentException("Connection string 'DBConnectionWithoutDB' is missing or empty in configuration.");
        this.scriptPath = configuration.GetSection("ScriptPath").Value ?? throw new ArgumentException("Script path 'ScriptPath' is missing or empty in configuration.");
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
        string scriptContent = File.ReadAllText(scriptPath);
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
