using Serilog;
using System;
using System.IO;
using System.Web.Http;

namespace HCeventviewer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Log.Logger = new LoggerConfiguration()
                                .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs//Logs-.txt"),rollingInterval: RollingInterval.Day)
                                .CreateLogger();

            UnityConfig.RegisterComponents();
        }
    }
}
