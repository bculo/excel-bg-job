namespace Excel.API.Extensions;

public static class FormFileExtensions
{
    public static async Task<Stream> ToStream(this IFormFile file, CancellationToken ct = default)
    {
        var ms = new MemoryStream();
        await file.CopyToAsync(ms, ct);
        ms.Seek(0, SeekOrigin.Begin);
        return ms;
    }
}