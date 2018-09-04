using System;
using BCS.Application.Domain;
using BCS.Db.EF;
using BCS.Application.Entity;

namespace BCS.Db.Repository
{
    public class LogRepository : ILogRepository
    {
        private BCSDbContext _dbContent;

        public LogRepository()
        {
            _dbContent = new BCSDbContext();
        }

        public void Log(string operation, string message)
        {
            ActivityLog log = new ActivityLog();
            log.LogId = Guid.NewGuid();
            log.LogDate = DateTime.Now;
            log.Operation = operation;
            log.Activity = message;

            _dbContent.ActivityLogs.Add(log);
            _dbContent.SaveChanges();
        }     
    }
}
