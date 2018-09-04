using System;
using BCS.Application.Domain;
using BCS.Db.Repository;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using BCS.Service;

namespace BCS
{
    public class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        public static IDependencyResolver Resolver()
        {
            return DependencyResolver.Current;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IEmployeeRepositoy, EmployeeRepo>();
            container.RegisterType<ILogRepository, LogRepository>();
            container.RegisterType<IMonitor, MonitoringService>();

            RegisterTypes(container);
            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}
