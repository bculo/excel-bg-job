namespace Excel.API.Interfaces.Excel;

public class ExcelPerson
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public int? Age { get; init; }
}

public interface IExcelReader
{
    List<ExcelPerson> Read(Stream stream);
}