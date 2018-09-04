using BCS.Application.Domain;
namespace BCS.Application.Service
{
    public class ActivityLoggingService
    {
        ILogRepository _logRepo;
        IMonitor _monitor;

        public ActivityLoggingService(ILogRepository logRepo, IMonitor monitor)
        {
            _logRepo = logRepo;
            _monitor = monitor;
        }

        public void LogActivity(string operation, string message)
        {
            _logRepo.Log(operation, message);
            _monitor.Log(operation, message);
        }
    }
}
