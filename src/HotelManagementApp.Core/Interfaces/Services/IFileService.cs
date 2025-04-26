namespace HotelManagementApp.Core.Interfaces.Services;

public interface IFileService
{
    string UploadFile(string folder, byte[] file, string extension);
    void DeleteFile(string folder, string fileName);
    byte[] GetFile(string folder, string fileName);
    string GetFileUrl(string folder, string fileName);
}
