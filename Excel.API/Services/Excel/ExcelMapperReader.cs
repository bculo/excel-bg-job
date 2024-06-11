using Excel.API.Interfaces.Excel;
using Ganss.Excel;

namespace Excel.API.Services.Excel;

public class ExcelMapperReader : IExcelReader
{
    public List<ExcelPerson> Read(Stream stream)
    {
        var excel = new ExcelMapper(stream) { HeaderRowNumber = 0 };

        excel.AddMapping<ExcelPerson>("First name", p => p.FirstName);
        excel.AddMapping<ExcelPerson>("Last name", p => p.LastName);
        excel.AddMapping<ExcelPerson>("Age", p => p.Age);

        return excel.Fetch<ExcelPerson>().ToList();
    }
    
    public async Task<List<ExcelPerson>> ReadAsync(Stream stream)
    {
        var excel = new ExcelMapper() { HeaderRowNumber = 0 };

        excel.AddMapping<ExcelPerson>("First name", p => p.FirstName);
        excel.AddMapping<ExcelPerson>("Last name", p => p.LastName);
        excel.AddMapping<ExcelPerson>("Age", p => p.Age);

        return (await excel.FetchAsync(stream, typeof(ExcelPerson))).OfType<ExcelPerson>().ToList();
    }
}