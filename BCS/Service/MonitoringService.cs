using System;
using BCS.Application.Domain;
using BCS.Hubs;
using Microsoft.AspNet.SignalR;

namespace BCS.Service
{
    public class MonitoringService : IMonitor
    {
        public void Log(string operation, string message)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MonitorHub>();
            hubContext.Clients.All.activityLog(operation, message);
        }
    }
}
