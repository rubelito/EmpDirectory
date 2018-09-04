using System;
namespace BCS.Application.Domain
{
    public interface IMonitor
    {
        void Log(string operation, string message);
    }
}
