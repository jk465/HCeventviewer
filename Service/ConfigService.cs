using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace HCeventviewer.Service
{
    public static class ConfigService
    {
        public static string[] LogNames { get; } = ConfigurationManager.AppSettings["LogName"].Split(';');
        private static string[] WorksheetNames { get; } = ConfigurationManager.AppSettings["WorksheetName"].Split(';');
        public static string[] MachineNames { get; } = ConfigurationManager.AppSettings["MachineName"].Split(';');
        public static string TemplateFilePath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EventLog_Template.xlsx");
        public static string OutputFilePath { get; } = SetOutputFilePath();
        public static string[] AuthorizedUsers { get; } = ConfigurationManager.AppSettings["AuthorizedUsers"].Split(';');
        public static Dictionary<string, string> WorksheetWithLog { get; } = SetWorksheetWithLog();

        private static Dictionary<string, string> SetWorksheetWithLog()
        {
            var WorksheetWithLog = new Dictionary<string, string>();
            for (int i = 0; i < LogNames.Length; i++)
            {
                WorksheetWithLog.Add(LogNames[i], WorksheetNames[i]);
            }

            return WorksheetWithLog;
        }

        public static string SetOutputFilePath()
        {
            var CurrentDate = DateTime.Today.ToString("dd_MMM_yyyy");
            var Filename = $"Prod_Web_All_WarnEr_{CurrentDate}.xlsx";
            var FilePath = Path.Combine(ConfigurationManager.AppSettings["OutputFilePath"].ToString(), Filename);

            return FilePath;
        }
    }

}