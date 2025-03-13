namespace NotificationService.DAL.DBManager;
public interface IDbManager
{
    Task CreateTableAsync();
    Task DropTableAsync();
}
