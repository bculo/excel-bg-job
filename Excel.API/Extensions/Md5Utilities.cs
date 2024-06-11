using System.Security.Cryptography;

namespace Excel.API.Extensions;

public static class Md5Utilities
{
    public static async Task<string> HashAsHexString(this Stream stream, CancellationToken ct = default)
    {
        using var md = MD5.Create();
        var hash = await md.ComputeHashAsync(stream, ct);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    } 
    
    public static async Task<byte[]> Hash(this Stream stream, CancellationToken ct = default)
    {
        using var md = MD5.Create();
        return await md.ComputeHashAsync(stream, ct);
    } 
}