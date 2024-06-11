using Excel.API.Interfaces.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Excel.API.Services.Excel;

public class NpoiReader  : IExcelReader
{
    public List<ExcelPerson> Read(Stream stream)
    {
        var xssWorkbook = new XSSFWorkbook(stream);
        var sheet = xssWorkbook.GetSheetAt(0);
        var headerRow = sheet.GetRow(0);
        int cellCount = headerRow.LastCellNum;
        
        // Get header values 
        // for (var j = 0; j < cellCount; j++)
        // {
        //     var cell = headerRow.GetCell(j);
        //     if (cell == null || string.IsNullOrWhiteSpace(cell.ToString()))
        //     {
        //         continue;
        //     }
        //     Console.WriteLine(cell.ToString());
        // }
        
        for (var i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
        {
            var row = sheet.GetRow(i);
            
            if (row == null)
            {
                continue;
            }
            
            if (row.Cells.All(d => d.CellType == CellType.Blank))
            {
                continue;
            }

            Console.WriteLine(row.GetCell(0)?.ToString());
            Console.WriteLine(row.GetCell(1)?.ToString());
            Console.WriteLine(row.GetCell(2)?.NumericCellValue);

            
            var person = new ExcelPerson
            {
                FirstName = row.GetCell(0)?.ToString(),
                LastName = row.GetCell(1)?.ToString(),
             };
            
        }

        return [];
    }
}