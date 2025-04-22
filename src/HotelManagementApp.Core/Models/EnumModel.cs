namespace HotelManagementApp.Core.Models;

public class EnumModel<TEnum> where TEnum : Enum
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public static ICollection<EnumModel<TEnum>> ParseEnumsToModel()
    {
        var output = new List<EnumModel<TEnum>>();
        var enumValues = Enum.GetValues(typeof(TEnum));
        foreach (var enumValue in enumValues)
        {
            var enumName = Enum.GetName(typeof(TEnum), enumValue)
                ?? throw new Exception($"Enum name not found for value {enumValue}");
            var enumId = Convert.ToInt32(enumValue);
            output.Add(new EnumModel<TEnum>
            {
                Id = enumId + 1,
                Name = enumName.ToUpper()
            });
        }
        return output;
    }
}
