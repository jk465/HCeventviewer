using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCeventviewer.Repository
{
    public interface IExcelService
    {
        Task<ExcelPackage> CreateExcelPackage(string templateFilePath);
        Task<string> SaveExcelPackage(ExcelPackage package, string outputFilePath);
    }
}
