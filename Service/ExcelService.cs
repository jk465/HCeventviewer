using HCeventviewer.Repository;
using OfficeOpenXml;
using System.IO;
using System.Threading.Tasks;

namespace HCeventviewer.Service
{
    public class ExcelService : IExcelService
    {
        public async Task<ExcelPackage> CreateExcelPackage(string templateFilePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            FileInfo ExcelTemplate = new FileInfo(templateFilePath);
            return await Task.FromResult(new ExcelPackage(ExcelTemplate));
        }

        public async Task<string> SaveExcelPackage(ExcelPackage package, string outputFilePath)
        {
            int fileVersion = 1;
            string newOutputFilePath = outputFilePath;

            //Logic to prevent override the existing file
            while (File.Exists(newOutputFilePath))
            {
                string directory = Path.GetDirectoryName(outputFilePath);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(outputFilePath);
                string extension = Path.GetExtension(outputFilePath);

                newOutputFilePath = Path.Combine(directory, $"{fileNameWithoutExtension} ({fileVersion}){extension}");
                fileVersion++;
            }

            await Task.Run(() => package.SaveAs(new FileInfo(newOutputFilePath)));

            return newOutputFilePath;
        }
    }
}