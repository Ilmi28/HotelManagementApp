using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Application.ValidationAttributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class ValidFileExtensionAttribute : ValidationAttribute
{
    private List<string> AllowedExtensions { get; set; } 

    public ValidFileExtensionAttribute(string fileExtensions)
    {
        AllowedExtensions = fileExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    public override bool IsValid(object? value)
    {
        IFormFile? file = value as IFormFile;

        if (file != null)
        {
            var fileName = file.FileName;

            return AllowedExtensions.Any(y => fileName.EndsWith(y));
        }

        return true;
    }
}
