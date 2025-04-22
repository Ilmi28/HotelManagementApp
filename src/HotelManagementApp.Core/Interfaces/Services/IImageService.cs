namespace HotelManagementApp.Core.Interfaces.Services;

public interface IImageService
{
    string UploadImage(byte[] image);
    void DeleteImage(string imageName);
    byte[] GetImage(string imageName);
}
