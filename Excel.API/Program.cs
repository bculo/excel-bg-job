using System.Reflection;
using System.Text;
using Azure.Storage.Blobs;
using FluentValidation;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.AspNetCore;
using Excel.API.Interfaces.Blob;
using Excel.API.Interfaces.Excel;
using Excel.API.Jobs;
using Excel.API.Options;
using Excel.API.Services.Blob;
using Excel.API.Services.Excel;

var builder = WebApplication.CreateBuilder(args);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions();

builder.Services.Configure<BlobStorageOptions>(builder.Configuration.GetSection("BlobStorageOptions"));

builder.Services.AddSingleton<IBlobStorage, BlobStorage>((provider) =>
{
    var options = provider.GetRequiredService<IOptions<BlobStorageOptions>>();
    var logger = provider.GetRequiredService<ILogger<BlobStorage>>();
    var storage = new BlobStorage(new BlobServiceClient(options.Value.ConnectionString), options, logger);
    storage.InitializeContext("test", true);
    return storage;
});

builder.Services.AddScoped<IExcelReader, ClosedXmlReader>();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey(nameof(ParseExcelFileJob));
    q.AddJob<ParseExcelFileJob>(opts => opts.WithIdentity(jobKey).StoreDurably());
});

builder.Services.AddQuartzServer(options =>
{
    options.WaitForJobsToComplete = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();