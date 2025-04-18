using HotelManagementApp.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace HotelManagementApp.Infrastructure.Services;

public class ImageService(IConfiguration config) : IImageService
{
    private readonly string basePath = config.GetValue<string>("ImagePath")
        ?? String.Empty;
    public void DeleteImage(string imageName)
    {
        var imagePath = Path.Combine(basePath, imageName);

        try
        {
            File.Delete(imagePath);
        }
        catch (DirectoryNotFoundException) { }
    }

    public byte[] GetImage(string imageName)
    {
        var imagePath = Path.Combine(basePath, imageName);

        if (!File.Exists(imagePath))
        {
            throw new FileNotFoundException("Image not found.");
        }
        return File.ReadAllBytes(imagePath);
    }

    public string UploadImage(byte[] image)
    {
        string imageName = $"{Guid.NewGuid()}.jpg";
        var imagePath = Path.Combine(basePath, imageName);
        try
        {
            File.WriteAllBytes(imagePath, image);
        }
        catch (Exception ex)
        {
            throw new FileLoadException($"Error uploading image: {ex.Message}");
        }
        return imageName;
    }
}
