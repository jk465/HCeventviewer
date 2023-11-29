using HCeventviewer.Repository;
using HCeventviewer.Service;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace HCeventviewer
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IEventLogs, EventLogService>();
            container.RegisterType<IExcelService, ExcelService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}