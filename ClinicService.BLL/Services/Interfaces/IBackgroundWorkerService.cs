namespace ClinicService.BLL.Services.Interfaces;

public interface IBackgroundWorkerService
{
    Task SendReminder(CancellationToken cancellationToken);
}