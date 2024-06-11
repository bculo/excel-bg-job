using System.Data;
using Excel.API.Interfaces.Excel;
using ExcelDataReader;

namespace Excel.API.Services.Excel;

public class ExcelDataReaderService  : IExcelReader
{
    public List<ExcelPerson> Read(Stream stream)
    {
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
        {
            ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
            {
                UseHeaderRow = true,
                ReadHeaderRow = (rowReader) => {
                    rowReader.Read();
                },
            }
        });
        
        foreach (DataTable table in result.Tables)
        {
            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    object item = row[column];
                    Console.WriteLine(item);
                }
            }
        }

        return [];
    }
    
}