using ClosedXML.Excel;
using Excel.API.Interfaces.Excel;
using FluentValidation;

namespace Excel.API.Services.Excel;

public class ClosedXmlReader : IExcelReader
{
    public List<ExcelPerson> Read(Stream stream)
    {
        List<ExcelPerson> persons = [];
        
        using var excelWorkbook = new XLWorkbook(stream);
        var nonEmptyDataRows = excelWorkbook.Worksheet(1).RowsUsed().Skip(1);
        
        foreach (var dataRow in nonEmptyDataRows)
        {
            persons.Add(new ExcelPerson
            {
                FirstName = dataRow.Cell(1).GetString(),
                LastName = dataRow.Cell(2).GetValue<string?>(),
                Age = dataRow.Cell(3).TryGetValue(out int? age) ? age : default
            });
            
        }
        
        return persons;
    }
}