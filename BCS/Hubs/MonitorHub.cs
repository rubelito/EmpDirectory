using Microsoft.AspNet.SignalR;

namespace BCS.Hubs
{
    [Authorize(Roles = "Admin")]
    public class MonitorHub : Hub
    {
        public void Send(string operation, string message)
        {
            Clients.All.activityLog(operation, message);
        }
    }
}