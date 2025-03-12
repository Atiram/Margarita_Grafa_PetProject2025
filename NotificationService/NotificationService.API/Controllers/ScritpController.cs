using Microsoft.AspNetCore.Mvc;
using NotificationService.DAL.DBManager;

namespace NotificationService.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ScritpController(IDbManager dbManager) : ControllerBase
{
    [HttpPost]
    public async Task Post()
    {
        await dbManager.CreateTableAsync();
    }

    [HttpDelete]
    public async Task Delete()
    {
        await dbManager.DropTableAsync();
    }
}
