using HotelManagementApp.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace HotelManagementApp.Infrastructure.Services;

public class LocalFileService(IConfiguration config) : IFileService
{
    private readonly string basePath = config.GetValue<string>("wwwrootPath")
        ?? String.Empty;
    public void DeleteFile(string folder, string fileName)
    {
        var imagePath = Path.Combine(basePath, folder, fileName);

        try
        {
            File.Delete(imagePath);
        }
        catch (DirectoryNotFoundException) { }
    }

    public byte[] GetFile(string folder, string fileName)
    {
        var imagePath = Path.Combine(basePath, folder, fileName);

        if (!File.Exists(imagePath))
        {
            throw new FileNotFoundException("Image not found.");
        }
        return File.ReadAllBytes(imagePath);
    }

    public string UploadFile(string folder, byte[] file, string extension)
    {
        string imageName = $"{Guid.NewGuid()}{extension}";
        var imagePath = Path.Combine(basePath, folder, imageName);
        try
        {
            File.WriteAllBytes(imagePath, file);
        }
        catch (Exception ex)
        {
            throw new FileLoadException($"Error uploading file: {ex.Message}");
        }
        return imageName;
    }

    public string GetFileUrl(string folder, string fileName)
    {
        var baseUrl = config["BaseUrl"];
        var link = $"{baseUrl}/{folder}/{fileName}";
        return link;
    }
}
