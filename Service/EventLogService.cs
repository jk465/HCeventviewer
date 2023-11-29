using HCeventviewer.Repository;
using OfficeOpenXml;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace HCeventviewer.Service
{
    public class EventLogService : IEventLogs
    {
        private readonly IExcelService excelService;
        public EventLogService(IExcelService _excelService)
        {
            excelService = _excelService;
        }

        public async Task<string> GetEventLogs(DateTime? fromDate, DateTime? toDate)
        {
            string OutputFilePath;

            if (!fromDate.HasValue || !toDate.HasValue)
            {
                fromDate = DateTime.Today.AddDays(-1).AddHours(1);
                toDate = DateTime.Today.AddSeconds(-1);
            }

            using (ExcelPackage package = await excelService.CreateExcelPackage(ConfigService.TemplateFilePath))
            {
                foreach (var machine in ConfigService.MachineNames)
                {
                    foreach (var log in ConfigService.LogNames)
                    {
                        ExcelWorksheet worksheet = GetWorkSheet(package, log);
                        await WriteEventData(log, machine, worksheet,fromDate,toDate);
                    }

                    Log.Logger.Information($"Completed for Machine {machine}");
                }
                OutputFilePath = await excelService.SaveExcelPackage(package,ConfigService.OutputFilePath);
            }

            return OutputFilePath;
        }

        private ExcelWorksheet GetWorkSheet(ExcelPackage package, string log)
        {
            var IsValidWorksheet = ConfigService.WorksheetWithLog.TryGetValue(log, out string WorksheetName);
            if (IsValidWorksheet)
                return package.Workbook.Worksheets[WorksheetName];

            return package.Workbook.Worksheets.Add(log);
        }

        private int GetLastRowIndex(ExcelWorksheet worksheet)
        {
            var LastRow = worksheet.Dimension?.End.Row ?? 1;

            for (int row = LastRow; row >= 1; row--)
            {
                var CellValue = worksheet.Cells[row, 1].Value;
                if (CellValue != null && !string.IsNullOrWhiteSpace(CellValue.ToString()))
                    return row + 1;
            }

            return 2;
        }

        private bool IsEntryTypeValid(EventLogEntryType entryType)
        {
            return entryType == EventLogEntryType.Error ||
                entryType == EventLogEntryType.Warning;
        }

        private async Task WriteEventData(string logName, string machineName, ExcelWorksheet worksheet, DateTime? fromDate, DateTime? toDate)
        {
            EventLog eventLog;
            try
            {
                eventLog = new EventLog(logName, machineName);
                int RowIndex = GetLastRowIndex(worksheet);

                var processingTasks = new List<Task>();

                foreach (EventLogEntry item in eventLog.Entries)
                {
                    if (item.TimeGenerated >= fromDate && item.TimeGenerated <= toDate)
                    {
                        if (IsEntryTypeValid(item.EntryType))
                        {
                            Task eachTask = Task.Run(() => UpdateWorksheetWithData(item, worksheet, RowIndex));
                            processingTasks.Add(eachTask);
                            RowIndex++;
                        }
                    }
                }
                await Task.WhenAll(processingTasks);
            }
            catch (ArgumentException e)
            {
                Log.Logger.Error(e,e.Message);
                return;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e,e.Message);
                return;
            }
        }

        private void UpdateWorksheetWithData(EventLogEntry item, ExcelWorksheet worksheet, int rowIndex)
        {
            worksheet.Cells[rowIndex, 1].Value = item.EntryType;
            worksheet.Cells[rowIndex, 2].Value = item.TimeGenerated;
            worksheet.Cells[rowIndex, 2].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
            worksheet.Cells[rowIndex, 3].Value = item.Source;
            worksheet.Cells[rowIndex, 4].Value = item.InstanceId & 0x3FFFFFFF;
            worksheet.Cells[rowIndex, 5].Value = item.Category;
            worksheet.Cells[rowIndex, 6].Value = item.MachineName;
            worksheet.Cells[rowIndex, 7].Value = item.Message;
        }
    }
}