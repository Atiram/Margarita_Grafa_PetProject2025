namespace OfficeService.DAL.Entities;
public record CityStreetPair
{
    public string City { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
}
