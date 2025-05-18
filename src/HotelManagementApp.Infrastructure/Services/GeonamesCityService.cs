using HotelManagementApp.Core.Interfaces.Services;
using HotelManagementApp.Core.Models.HotelModels;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

namespace HotelManagementApp.Infrastructure.Services;

public class GeonamesCityService(HttpClient httpClient) : ICityService
{
    public async IAsyncEnumerable<City> FetchCities([EnumeratorCancellation] CancellationToken ct = default)
    {
        await using var stream = await httpClient.GetStreamAsync("https://public.opendatasoft.com/api/explore/v2.0/catalog/datasets/geonames-all-cities-with-a-population-1000/exports/json?limit=-1&timezone=UTC&use_labels=false&epsg=4326", ct);
        var json = await JsonNode.ParseAsync(stream, cancellationToken: ct);
        foreach (var item in json!.AsArray())
        {
            var isCountryNull = item!["cou_name_en"] == null;
            var isIdNull = item["geoname_id"] == null;
            var isNameNull = item["ascii_name"] == null;
            var isLatitudeNull = item["coordinates"]!["lat"] == null;
            var isLongitudeNull = item["coordinates"]!["lon"] == null;
            if (isCountryNull || isIdNull || isNameNull || isLatitudeNull || isLongitudeNull)
                continue;
            yield return new City
            {
                Id = int.Parse(item["geoname_id"]!.GetValue<string>()),
                Name = item["ascii_name"]!.GetValue<string>(),
                Latitude = item["coordinates"]!["lat"]!.GetValue<double>(),
                Longitude = item["coordinates"]!["lon"]!.GetValue<double>(),
                Country = item["cou_name_en"]!.GetValue<string>().Normalize()
            };
        }
    }
}
