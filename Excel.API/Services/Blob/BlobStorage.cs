using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Excel.API.Interfaces.Blob;
using Excel.API.Options;
using Microsoft.Extensions.Options;

namespace Excel.API.Services.Blob;

public class BlobStorage(
    BlobServiceClient blobServiceClient,
    IOptions<BlobStorageOptions> options,
    ILogger<BlobStorage> logger)
    : IBlobStorage
{
    private readonly BlobStorageOptions _options = options.Value;

    public void InitializeContext(string containerName, bool isPublic = false)
    {
        try
        {
            var accessType = isPublic ? PublicAccessType.Blob : PublicAccessType.None;
            blobServiceClient.CreateBlobContainer(containerName, accessType);
        }
        catch
        {
            logger.LogWarning("Container {ContainerName} already created", containerName);
        }
    }

    private BlobClient GetBlobClient(string containerName, string blobIdentifier)
    {
        return blobServiceClient.GetBlobContainerClient(containerName)
            .GetBlobClient(blobIdentifier);
    }
    
    public async Task<Uri> UploadBlob(string containerName, 
        string blobIdentifier, Stream blob, 
        string contentType,
        CancellationToken ct = default)
    {
        // azure blob storage automatically calculates md5 hash of uploaded file
        //var md5Hash = await Md5Utilities.Hash(blob, ct);
        
        var httpHeaders = new BlobHttpHeaders
        {
            ContentType = contentType,
            ContentLanguage = "en",
            //ContentHash = md5Hash
        };
        
        var blobClient = GetBlobClient(containerName, blobIdentifier);
        await blobClient.UploadAsync(blob, httpHeaders: httpHeaders, cancellationToken: ct);
        return blobClient.Uri;
    }

    public async Task<Stream> DownloadBlob(string containerName, string blobIdentifier, CancellationToken ct = default)
    {
        var stream = new MemoryStream();
        var blobClient = GetBlobClient(containerName, blobIdentifier);
        var response = await blobClient.DownloadToAsync(stream, ct);
        if (response.IsError)
        {
            throw new ArgumentException("Item not found");
        }
        return stream;
    }
}