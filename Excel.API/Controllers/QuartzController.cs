using Microsoft.AspNetCore.Mvc;
using Quartz;
using Excel.API.Extensions;
using Excel.API.Interfaces.Blob;
using Excel.API.Jobs;

namespace Excel.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuartzController(ISchedulerFactory schedulerFactory, IBlobStorage storage) : ControllerBase
{
    [HttpPost("upload-document")]
    public async Task<IActionResult> UploadDocument(IFormFile file, CancellationToken ct)
    {
        await using var ms = await file.ToStream(ct);
        var fileIdentifier = Guid.NewGuid().ToString().Replace("-", "");
        await storage.UploadBlob("test", fileIdentifier, ms, file.ContentType, ct);
        
        var jobData = new JobDataMap();
        jobData.Put("DocumentId", fileIdentifier);
        var scheduler = await schedulerFactory.GetScheduler(ct);
        await scheduler.TriggerJob(new JobKey(nameof(ParseExcelFileJob)), jobData, ct);
        
        return Ok(fileIdentifier);
    }

    [HttpPost("hash-file")]
    public async Task<IActionResult> HashFile(IFormFile file, CancellationToken ct)
    {
        await using var ms = await file.ToStream(ct);
        var hash = await ms.HashAsHexString(ct);
        return Ok(hash);
    }
}

