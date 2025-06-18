using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Soditech.IntelPrev.Reports.Application;

public class NotificationsHub: Hub<INotificationClient>
{

}

public interface INotificationClient
{
    Task ReceiveNotification(string message);
}

