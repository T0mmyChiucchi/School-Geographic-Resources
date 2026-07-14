namespace SchoolGeoResources.Infrastructure.Services;

using Microsoft.Extensions.Configuration;
using SchoolGeoResources.Application.Common.Interfaces;
using Supabase;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class SupabaseStorageService : IStorageService
{
    private readonly Client _supabaseClient;
    private readonly string _bucketName;

    public SupabaseStorageService(IConfiguration configuration)
    {
        var url = configuration["Supabase:Url"] ?? "https://your-project.supabase.co";
        var key = configuration["Supabase:Key"] ?? "your-anon-or-service-role-key";
        _bucketName = configuration["Supabase:Bucket"] ?? "resources";

        var options = new SupabaseOptions { AutoConnectRealtime = false };
        _supabaseClient = new Client(url, key, options);
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream, cancellationToken);
        var fileBytes = memoryStream.ToArray();

        var storageOptions = new Supabase.Storage.FileOptions { ContentType = contentType };
        
        var bucket = _supabaseClient.Storage.From(_bucketName);
        
        var uniqueFileName = $"{Guid.NewGuid()}-{fileName}";
        await bucket.Upload(fileBytes, uniqueFileName, storageOptions);

        return bucket.GetPublicUrl(uniqueFileName);
    }

    public async Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        var bucket = _supabaseClient.Storage.From(_bucketName);
        var uri = new Uri(fileUrl);
        var fileName = Path.GetFileName(uri.LocalPath);
        
        await bucket.Remove(new System.Collections.Generic.List<string> { fileName });
    }
}
