using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HCeventviewer.Service
{
    public static class LogFileCleanUpService
    {
        public static void CleanUpLogFiles()
        {
            try
            {
                DateTime ThresholdDate = DateTime.Now.AddDays(-3);

                string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

                var IsExist = Directory.Exists(LogDirectory);

                if (!IsExist)
                    return;

                string[] LogFiles = Directory.GetFiles(LogDirectory, "Logs*.txt");

                foreach (var file in LogFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);

                    if (fileInfo.CreationTimeUtc < ThresholdDate)
                        fileInfo.Delete();
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e,e.Message);
            }
        }
    }
}