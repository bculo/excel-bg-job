namespace Excel.API.Interfaces.Blob;

public interface IBlobStorage
{
    Task<Uri> UploadBlob(string containerName, 
        string blobIdentifier, 
        Stream blob,  
        string contentType, 
        CancellationToken ct = default);

    Task<Stream> DownloadBlob(string containerName,
        string blobIdentifier,
        CancellationToken ct = default);
}