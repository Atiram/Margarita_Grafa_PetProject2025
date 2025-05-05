using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicService.BLL.Services.Interfaces;

public interface IBackgroundWorkerService
{
    Task SendReminder(CancellationToken cancellationToken);
}