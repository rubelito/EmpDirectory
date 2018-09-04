using System;
namespace BCS.Application.Domain
{
    public interface ILogRepository
    {
         void Log(string operation, string message);
    }
}