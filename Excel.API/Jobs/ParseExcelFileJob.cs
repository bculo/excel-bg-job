using System.Text.Json;
using Excel.API.Interfaces.Blob;
using Excel.API.Interfaces.Excel;
using FluentValidation;
using Quartz;

namespace Excel.API.Jobs;

public class ParseExcelFileJob(IBlobStorage storage, IExcelReader reader, IValidator<ExcelPerson> validator) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"HELLO THERE. JOB IS EXECUTED ON {DateTime.UtcNow}");
        
        var documentId = context.MergedJobDataMap.Get("DocumentId") as string;
        await using var file = await storage.DownloadBlob("test", documentId!);
        var t = reader.Read(file);
        
        Console.WriteLine(JsonSerializer.Serialize(t));

        var validOnes = t.Where(i => validator.Validate(i).IsValid).ToList();
        
        Console.WriteLine(JsonSerializer.Serialize(validOnes));

        // SEND NOTIFICATION TO USER 
    }
}